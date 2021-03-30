using CQRS_Simple.Products.API.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace CQRS_Simple.Products.API.EntityFrameworkCore
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