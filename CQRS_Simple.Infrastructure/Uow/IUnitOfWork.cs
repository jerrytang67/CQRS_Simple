using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CQRS_Simple.Infrastructure.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void PrintKey();
    }
}