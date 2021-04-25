using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FileSystemSearchService.Core.Interfaces.Repository.Elastic
{
    public abstract class ElasticRepository<TEntity>
        : IElasticRepository<TEntity, string> where TEntity : class
    {
        protected readonly IElasticClient _elasticClient;
        protected readonly string _indexName;

        public ElasticRepository(IElasticClient elasticClient, string indexName)
        {
            _elasticClient = elasticClient;
            _indexName = indexName;
        }

        //Create
        public virtual string Add(TEntity entity)
        {
            //todo: Find a way to test when unable to index / add.
            var indexResponse = _elasticClient
                .IndexDocument(entity);

            return indexResponse.Id;
        }

        public virtual bool BulkAdd(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        //Read
        public virtual TEntity Get(string id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        //Update
        public virtual TEntity Update(string id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        //Delete
        public virtual bool Delete(string id)
        {
            var deleteResponse = _elasticClient
                .Delete<TEntity>(id);

            return deleteResponse.Result == Result.Deleted;
        }
    }
}
