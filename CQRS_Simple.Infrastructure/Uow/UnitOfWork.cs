using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CQRS_Simple.Infrastructure.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IIocManager _iocManager;
        private readonly ILogger<UnitOfWork> _log;
        private Guid KEY { get; }
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

            KEY = Guid.NewGuid();

            _log.LogDebug($"UnitOfWork Init {KEY}");
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
            _log.LogInformation($"PrintKey:{KEY}");
        }

        /// <summary>
        /// 手动清理
        /// </summary>
        public void CleanUp()
        {
            Commit();

            Transaction?.Dispose();
            Context?.Dispose();

            _log.LogDebug($"Context CleanUp");
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
                _log.LogError("Context Transaction Error");
                _log.LogError(e.Message);
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
                _log.LogError("Context Transaction Error");
                _log.LogError(e.Message);
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