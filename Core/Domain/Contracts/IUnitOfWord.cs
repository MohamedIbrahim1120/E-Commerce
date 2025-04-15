using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWord
    {
        Task<int> SaveChangesAsync();

        // Generate Repository

        IGenericRepository<TEntity, TKey> GetRepository<TEntity,TKey>() where TEntity : BaseEntity<TKey>;

         
    }
}
