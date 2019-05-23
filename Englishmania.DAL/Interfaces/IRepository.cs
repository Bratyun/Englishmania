using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Englishmania.DAL.Entities;

namespace Englishmania.DAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        List<T> GetAll();
        T Get(int id);
        T Get(Expression<Func<T, bool>> predicate);
        List<T> Find(Expression<Func<T, bool>> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Delete(T entity);
    }
}