using Nest;
using System;

namespace FileSystemSearchService.Core.Entities
{
    [ElasticsearchType(IdProperty = nameof(FullPath))]
    public class Artifact
    {
        public string FullPath { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FileType { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime LastAccessed { get; set; }
        public long Bytes { get; set; }
    }
}
