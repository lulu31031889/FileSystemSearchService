using FileSystemSearchService.Core.DTO.FileSystem;
using FileSystemSearchService.Core.Interfaces.FileSystem;
using FileSystemSearchService.Core.Interfaces.Services;
using System;
using System.IO;

namespace FileSystemSearchService.Infrastructure.Services
{
    public class FileSystemEventFactory : IFileSystemEventFactory
    {
        readonly IFilesAndFoldersService _filesAndFoldersService;

        public FileSystemEventFactory(IFilesAndFoldersService filesAndFoldersService)
        {
            _filesAndFoldersService = filesAndFoldersService;
        }

        public IFileSystemChangedEventDTO GenerateRelevantEvent(FileSystemEventArgs eventArgs)
        {
            var IsFileOrFolder = _filesAndFoldersService.IsFileOrFolder(eventArgs.FullPath);

            switch (eventArgs.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    return new CreatedEventDTO
                    {
                        ArtifactType = IsFileOrFolder,
                        FullPath = eventArgs.FullPath,
                        Name = eventArgs.Name
                    };
                case WatcherChangeTypes.Deleted:
                    return new DeletedEventDTO
                    {
                        ArtifactType = IsFileOrFolder,
                        FullPath = eventArgs.FullPath,
                        Name = eventArgs.Name
                    };
                case WatcherChangeTypes.Changed:
                    return new ChangedEventDTO
                    {
                        ArtifactType = IsFileOrFolder,
                        FullPath = eventArgs.FullPath,
                        Name = eventArgs.Name
                    };
                //Handled below.
                //case WatcherChangeTypes.Renamed:
                //    break;
                //todo: Decide what to do with WatcherChangeTypes.All
                case WatcherChangeTypes.All:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public IFileSystemChangedEventDTO GenerateRelevantEvent(RenamedEventArgs eventArgs)
        {
            var IsFileOrFolder = _filesAndFoldersService.IsFileOrFolder(eventArgs.FullPath);

            return new RenamedEventDTO
            {
                ArtifactType = IsFileOrFolder,
                FullPath = eventArgs.FullPath,
                Name = eventArgs.Name,
                OldFullPath = eventArgs.OldFullPath,
                OldName = eventArgs.OldName
            };
        }

        public IFileSystemChangedEventDTO GenerateRelevantEvent(ErrorEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
