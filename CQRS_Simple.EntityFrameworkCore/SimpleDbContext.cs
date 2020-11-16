using CQRS_Simple.Domain.Products;
using Microsoft.EntityFrameworkCore;

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
        }
    }
}