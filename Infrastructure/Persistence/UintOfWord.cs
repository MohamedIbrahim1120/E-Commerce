using Domain.Contracts;
using Domain.Models;
using Persistence.Data;
using Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UintOfWord : IUnitOfWord
    {
        private readonly StoreDbContext _context;
        private readonly ConcurrentDictionary<string,object> _repositries;

        public UintOfWord(StoreDbContext context)
        {
            _context = context;
            _repositries = new ConcurrentDictionary<string,object>();
        }

        //public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        //{
        //    var type = typeof(TEntity).Name;
        //    if (!_repositries.ContainsKey(type))
        //    {
        //        var repository = new GenericRepository<TEntity,TKey>(_context);
        //        _repositries.Add(type, repository);

        //    }

        //    return (IGenericRepository<TEntity, TKey>)_repositries[type];
        //}

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
          => (IGenericRepository<TEntity, TKey>) 
            _repositries.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_context));
        


        public async Task<int> SaveChangesAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
