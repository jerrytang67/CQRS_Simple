using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Simple.Core.Uow
{
    public class Repository<T, TPrimaryKey> : IRepository<T, TPrimaryKey> where T : Entity<TPrimaryKey>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public T GetById(TPrimaryKey id)
        {
            return _unitOfWork.Context.Set<T>().FirstOrDefault(x => id.Equals(x.Id));
        }

        public Task<T> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken)
        {
            return _unitOfWork.Context.Set<T>().FirstOrDefaultAsync(x => id.Equals(x.Id), cancellationToken);
        }

        public IQueryable<T> GetAll()
        {
            return _unitOfWork.Context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork.Context.Set<T>().Where(predicate);
        }

        public void Update(T entity)
        {
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.Context.Set<T>().Attach(entity);
        }
    }
}