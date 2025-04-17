using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        #region Demo01
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return trackChanges ?
                          await _context.products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync() as IEnumerable<TEntity>
                        : await _context.products.Include(p => p.ProductBrand).Include(p => p.ProductType).AsNoTracking().ToListAsync() as IEnumerable<TEntity>;
            }

            return trackChanges ?
                await _context.Set<TEntity>().ToListAsync() :
                await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        #endregion

        public async Task<TEntity> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return await _context.products.Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync(p => p.Id == id as int?) as TEntity;

            }
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task AddAsync(TEntity entity)
        {
             await _context.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }



        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> spec, bool trackChanges = false)
        {
           return await ApllySepcifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecification<TEntity, TKey> spec)
        {
           return await ApllySepcifications(spec).FirstOrDefaultAsync();
        }

        private IQueryable<TEntity> ApllySepcifications(ISpecification<TEntity,TKey> spec)
        {
            return SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }

        public async Task<int> CountAsyn(ISpecification<TEntity, TKey> spec)
        {
            return await ApllySepcifications(spec).CountAsync();
        }

    } 
}
