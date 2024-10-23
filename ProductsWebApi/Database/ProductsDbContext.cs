using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Database.Models;

namespace ProductsWebApi.Database;

public class ProductsDbContext : IdentityDbContext<User>
{
    public virtual DbSet<Product> Products { get; set; }
    
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
    : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Product>().HasKey(p => p.Id);
    }
}