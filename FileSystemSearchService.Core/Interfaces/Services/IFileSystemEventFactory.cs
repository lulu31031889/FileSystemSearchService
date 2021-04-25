using FileSystemSearchService.Core.Interfaces.FileSystem;
using System.IO;

namespace FileSystemSearchService.Core.Interfaces.Services
{
    public interface IFileSystemEventFactory
    {
        IFileSystemChangedEventDTO GenerateRelevantEvent(FileSystemEventArgs eventArgs);
        IFileSystemChangedEventDTO GenerateRelevantEvent(RenamedEventArgs eventArgs);
        IFileSystemChangedEventDTO GenerateRelevantEvent(ErrorEventArgs eventArgs);
    }
}
