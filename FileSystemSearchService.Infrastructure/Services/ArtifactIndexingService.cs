using FileSystemSearchService.Core.DTO.FileSystem;
using FileSystemSearchService.Core.Entities;
using FileSystemSearchService.Core.Extensions;
using FileSystemSearchService.Core.Interfaces.FileSystem;
using FileSystemSearchService.Core.Interfaces.Services;
using FileSystemSearchService.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.IO.Abstractions;

namespace FileSystemSearchService.Infrastructure.Services
{
    public class ArtifactIndexingService : IArtifactIndexingService
    {
        readonly ILogger<ArtifactIndexingService> _logger;

        readonly ArtifactRepository _artifactRepository;
        readonly IFilesAndFoldersService _filesAndFoldersService;

        public ArtifactIndexingService(ILogger<ArtifactIndexingService> logger,
            ArtifactRepository artifactRepository,
            IFilesAndFoldersService filesAndFoldersService)
        {
            _logger = logger;
            _artifactRepository = artifactRepository;
            _filesAndFoldersService = filesAndFoldersService;
        }

        public void DeltaUpdate(IFileSystemChangedEventDTO changedEvent)
        {
            var fileInfo = _filesAndFoldersService.GetFileInfo(changedEvent.FullPath);

            if (changedEvent.GetType() == typeof(CreatedEventDTO))
            {
                AddArtifact(changedEvent, fileInfo);
            }
            else if (changedEvent.GetType() == typeof(ChangedEventDTO))
            {
                //Upsert
            }
            else if (changedEvent.GetType() == typeof(RenamedEventDTO))
            {
                DeleteArtifact(changedEvent);

                AddArtifact(changedEvent, fileInfo);
            }
            else if (changedEvent.GetType() == typeof(DeletedEventDTO))
            {
                DeleteArtifact(changedEvent);
            }
            else
            {
                _logger.LogError("An unrecognized IChangedEvent implementation was received.", changedEvent.GetType().ToString());
                throw new ArgumentException("An unrecognized IChangedEvent implementation was received.", changedEvent.GetType().ToString());
            }
        }

        void DeleteArtifact(IFileSystemChangedEventDTO changedEvent)
        {
            //Delete
            //todo: When it's a rename, OldFullPath is used.  When "true" delete FullPath is used. (Rework this).
            var idOfArtifactToDelete = changedEvent.OldFullPath ?? changedEvent.FullPath;

            _artifactRepository.Delete(idOfArtifactToDelete);
        }

        void AddArtifact(IFileSystemChangedEventDTO changedEvent, IFileInfo fileInfo)
        {
            //Add
            var artifactToAdd = new Artifact
            {
                Bytes = fileInfo.Length,
                Created = fileInfo.CreationTime.ToElasticSearchNestDateTime(),
                FileType = fileInfo.Extension,
                FullPath = changedEvent.FullPath,
                LastAccessed = fileInfo.LastAccessTime.ToElasticSearchNestDateTime(),
                Modified = fileInfo.LastWriteTime.ToElasticSearchNestDateTime(),
                Name = changedEvent.Name,
                Path = fileInfo.DirectoryName
            };

            _artifactRepository.Add(artifactToAdd);
        }

        public void Rebuild()
        {
            throw new NotImplementedException();
        }
    }
}
