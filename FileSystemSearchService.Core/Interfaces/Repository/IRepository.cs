using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FileSystemSearchService.Core.Interfaces.Repository
{
    public interface IRepository<TEntity, TKey>
    {
        //Create
        TKey Add(TEntity entity);

        //Read
        TEntity Get(TKey id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        //Update
        TEntity Update(TKey id, TEntity entity);

        //Delete
        bool Delete(TKey id);
    }
}
