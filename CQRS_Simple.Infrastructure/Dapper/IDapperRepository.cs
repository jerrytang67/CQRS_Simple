using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CQRS_Simple.Infrastructure.Dapper
{
    public interface IDapperRepository<T, TC> where T : Entity<TC>
    {
        Task<TC> AddAsync(T item);
        Task<int> RemoveAsync(T item);
        Task<int> UpdateAsync(T item);
        Task<T> GetByIdAsync(TC id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
    }
}