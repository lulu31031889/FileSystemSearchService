using FileSystemSearchService.Core.Enums;
using System;

namespace FileSystemSearchService.Core.Interfaces.FileSystem
{
    public interface IFileSystemChangedEventDTO
    {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string OldFullPath { get; set; }
        public string OldName { get; set; }
        public DateTime EventRaisedDateTime { get; }
        public ArtifactType ArtifactType { get; set; }
    }
}
