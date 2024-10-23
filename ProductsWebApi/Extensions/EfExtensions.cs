using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Database;

namespace ProductsWebApi.Extensions;

public static class EfExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder modelBuilder)
    {
        using var serviceScope = modelBuilder.ApplicationServices.CreateScope();
        
        using var context = serviceScope.ServiceProvider.GetRequiredService<ProductsDbContext>();
        
        context.Database.Migrate();
    }
}