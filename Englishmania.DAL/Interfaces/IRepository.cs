using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Englishmania.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll();
        T Get(int id);
        T Get(Expression<Func<T, bool>> predicate);
        IList<T> Find(Expression<Func<T, bool>> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
