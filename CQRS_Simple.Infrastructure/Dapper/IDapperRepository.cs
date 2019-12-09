using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CQRS_Simple.Infrastructure.Dapper
{
    public interface IDapperRepository<T, C> where T : Entity<C>
    {
        Task AddAsync(T item);
        Task RemoveAsync(T item);
        Task UpdateAsync(T item);
        Task<T> GetByIdAsync(C id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
    }
}