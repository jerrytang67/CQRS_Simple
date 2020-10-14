using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CQRS_Simple.Infrastructure.Uow
{
    public interface IRepository<T, TC> where T : Entity<TC>
    {
        IUnitOfWork UnitOfWork { get; }
        T GetById(TC id);
        Task<T> GetByIdAsync(TC id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        
        Task<IEnumerable<T>> GetAllAsync();
    }
}