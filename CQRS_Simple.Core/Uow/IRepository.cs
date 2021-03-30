using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_Simple.Core.Uow
{
    public interface IRepository<T, TC> where T : Entity<TC>
    {
        IUnitOfWork UnitOfWork { get; }
        T GetById(TC id);
        Task<T> GetByIdAsync(TC id, CancellationToken cancellationToken);
        IQueryable<T> GetAll();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}