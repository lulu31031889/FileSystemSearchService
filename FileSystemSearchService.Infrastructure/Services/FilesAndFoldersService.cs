using FileSystemSearchService.Core.Enums;
using FileSystemSearchService.Core.Interfaces.Services;
using System.IO.Abstractions;

namespace FileSystemSearchService.Infrastructure.Services
{
    public class FilesAndFoldersService : IFilesAndFoldersService
    {
        readonly IFileSystem _fileSystem;

        public FilesAndFoldersService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public virtual ArtifactType IsFileOrFolder(string fullPath)
        {
            //todo: BUG - When deleting, because file doesn't exist, it defaults to being a folder.
            if (_fileSystem.File.Exists(fullPath))
            {
                return ArtifactType.File;
            }
            else
            {
                return ArtifactType.Folder;
            }
        }

        public virtual IFileInfo GetFileInfo(string fullPath)
        {
            //Assumption: File actually exists.
            //todo: Handle when can't find file / file doesn't exist.
            var fileInfo = _fileSystem.FileInfo.FromFileName(fullPath);
            return fileInfo;
        }
    }
}
