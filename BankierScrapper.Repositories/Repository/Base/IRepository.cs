using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BankierScrapper.Repositories.Repository.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
    }
}
