using FileSystemSearchService.Core.Enums;
using System.IO.Abstractions;

namespace FileSystemSearchService.Core.Interfaces.Services
{
    public interface IFilesAndFoldersService
    {
        ArtifactType IsFileOrFolder(string fullPath);
        IFileInfo GetFileInfo(string fullPath);
    }
}
