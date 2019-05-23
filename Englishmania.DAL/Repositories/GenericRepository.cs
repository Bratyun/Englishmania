using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Englishmania.DAL.EF;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Englishmania.DAL.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly EnglishmaniaContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(EnglishmaniaContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = Get(id);
            if (item != null) _dbSet.Remove(item);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}