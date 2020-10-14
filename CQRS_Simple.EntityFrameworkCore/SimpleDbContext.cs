using System;
using CQRS_Simple.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CQRS_Simple.EntityFrameworkCore
{
    public class SimpleDbContext : DbContext
    {
        protected SimpleDbContext()
        {
        }

        public DbSet<Product> Products { get; set; }

        public SimpleDbContext(DbContextOptions<SimpleDbContext> options)
            : base(options)
        {
#if DEBUG
            Log.Debug($"SimpleDbContext Init");
#endif
        }
    }
}