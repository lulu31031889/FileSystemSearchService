using FileSystemSearchService.Core.Entities;
using FileSystemSearchService.Core.Enums;
using FileSystemSearchService.Core.Interfaces.Repository.Elastic;
using Nest;

namespace FileSystemSearchService.Infrastructure.Repositories
{
    public class ArtifactRepository : ElasticRepository<Artifact>
    {
        public ArtifactRepository(IElasticClient _elasticClient)
            : base(_elasticClient, DocumentIndexNames.artifacts.ToString())
        { }
    }
}
