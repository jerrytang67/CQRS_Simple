using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CQRS_Simple.Infrastructure.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private Guid KEY { get; }

        public DbContext Context { get; }

        public UnitOfWork(DbContext context)
        {
            Context = context;
            KEY = Guid.NewGuid();
#if DEBUG
            Console.WriteLine($"UnitOfWork Init {KEY}");
#endif
        }
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public void PrintKey()
        {
            Log.Information($"PrintKey:{KEY}");
        }

        public void Dispose()
        {
            Context?.Dispose();
            Log.Information($"Context Dispose");
        }
    }
}