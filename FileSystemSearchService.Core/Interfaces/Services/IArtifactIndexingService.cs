using FileSystemSearchService.Core.Interfaces.FileSystem;

namespace FileSystemSearchService.Core.Interfaces.Services
{
    public interface IArtifactIndexingService
    {
        void DeltaUpdate(IFileSystemChangedEventDTO changedEvent);
        void Rebuild();
    }
}
