using FileSystemSearchService.Core.Enums;
using FileSystemSearchService.Core.Interfaces.FileSystem;
using System;

namespace FileSystemSearchService.Core.DTO.FileSystem
{
    public class RenamedEventDTO : IFileSystemChangedEventDTO
    {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string OldFullPath { get; set; }
        public string OldName { get; set; }
        public DateTime EventRaisedDateTime { get { return DateTime.Now; } }
        public ArtifactType ArtifactType { get; set; }
    }
}
