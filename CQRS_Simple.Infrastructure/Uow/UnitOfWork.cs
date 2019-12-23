using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace CQRS_Simple.Infrastructure.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IIocManager _iocManager;
        private Guid KEY { get; }

        public DbContext Context { get; }

        public IDbContextTransaction Transaction;

        public UnitOfWork(DbContext context, IIocManager iocManager)
        {
            _iocManager = iocManager;
            Context = context;

            Transaction = context.Database.BeginTransaction();

            KEY = Guid.NewGuid();
#if DEBUG
            Log.Information($"UnitOfWork Init {KEY}");
#endif
        }
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await CommitAsync();
        }

        public IRepository<T, TC> GetRepository<T, TC>() where T : Entity<TC>
        {
            return _iocManager.GetInstance<IRepository<T, TC>>();
        }

        public void PrintKey()
        {
            Log.Information($"PrintKey:{KEY}");
        }

        /// <summary>
        /// 手动清理
        /// </summary>
        public void CleanUp()
        {
            Commit();

            Transaction?.Dispose();
            Context?.Dispose();

#if DEBUG
            Log.Information($"Context CleanUp");
#endif
        }


        private int Commit()
        {
            var result = 0;
            try
            {
                result = Context.SaveChanges();
                Transaction?.Commit();
                return result;
            }
            catch (Exception e)
            {
                result = -1;
                Transaction?.Rollback();
                Log.Error("Context Transaction Error");
                Log.Error(e.Message);
            }
            return result;
        }
        private async Task<int> CommitAsync()
        {
            var result = 0;
            try
            {
                result = await Context.SaveChangesAsync();
                Transaction?.Commit();
                return result;
            }
            catch (Exception e)
            {
                result = -1;
                if (Transaction != null)
                    await Transaction.RollbackAsync();
                Log.Error("Context Transaction Error");
                Log.Error(e.Message);
            }
            return result;
        }



        public void Dispose()
        {
            Context?.Dispose();
#if DEBUG
            Log.Information($"Context Dispose");

#endif
        }
    }
}