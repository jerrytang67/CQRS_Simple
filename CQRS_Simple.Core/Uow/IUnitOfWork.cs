using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CQRS_Simple.Core.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();

        IRepository<T, TC> GetRepository<T, TC>() where T : Entity<TC>;
        void PrintKey();
    }
}