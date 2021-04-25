using System.Collections.Generic;

namespace FileSystemSearchService.Core.Interfaces.Repository.Elastic
{
    interface IElasticRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    {
        bool BulkAdd(IEnumerable<TEntity> entities);
    }
}
