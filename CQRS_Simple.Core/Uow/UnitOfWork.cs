using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CQRS_Simple.Core.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IIocManager _iocManager;
        private readonly ILogger<UnitOfWork> _log;
        private Guid Key { get; }
        public DbContext Context { get; }
        public IDbContextTransaction Transaction;

        public UnitOfWork(DbContext context,
            IIocManager iocManager,
            ILogger<UnitOfWork> log
        )
        {
            _iocManager = iocManager;
            _log = log;
            Context = context;

            Transaction = context.Database.BeginTransaction();

            Key = Guid.NewGuid();

            _log.LogDebug("UnitOfWork Init {Key}", Key);
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
            _log.LogInformation("PrintKey:{KEY}", Key);
        }

        /// <summary>
        /// 手动清理
        /// </summary>
        public void ReleaseCleanUp()
        {
            Commit();
            Transaction?.Dispose();
            _log.LogDebug("Context ReleaseCleanUp");
        }


        private int Commit()
        {
            var result = 0;
            try
            {
                result = Context.SaveChanges();
                Context.Database.CurrentTransaction?.Commit();
                return result;
            }
            catch (Exception e)
            {
                result = -1;
                Context.Database.CurrentTransaction?.Rollback();
                _log.LogError(e, "Context Transaction Error");
            }

            return result;
        }

        private async Task<int> CommitAsync()
        {
            var result = 0;
            try
            {
                result = await Context.SaveChangesAsync();
                await Transaction?.CommitAsync();
                return result;
            }
            catch (Exception e)
            {
                result = -1;
                if (Transaction != null)
                    await Transaction.RollbackAsync();
                _log.LogError(e, "Context Transaction Error");
            }

            return result;
        }


        public void Dispose()
        {
            Context?.Dispose();
            _log.LogDebug($"Context Dispose");
        }
    }
}