﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CQRS_Simple.Infrastructure.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private Guid KEY { get; }

        public DbContext Context { get; }

        public UnitOfWork(DbContext context)
        {
            Context = context;
            KEY = Guid.NewGuid();
#if DEBUG
            Log.Information($"UnitOfWork Init {KEY}");
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

        /// <summary>
        /// 手动清理
        /// </summary>
        public void CleanUp()
        {
            Context?.SaveChanges();
            Context?.Dispose();

#if DEBUG
            Log.Information($"Context CleanUp");
#endif
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