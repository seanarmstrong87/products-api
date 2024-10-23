using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Database;
using ProductsWebApi.Database.Models;
using ProductsWebApi.Interfaces;
using ProductsWebApi.Services;

namespace ProductsWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication()
            .AddCookie(IdentityConstants.ApplicationScheme, options =>
            {
                // Return to 401 when not authenticated
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

        builder.Services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<ProductsDbContext>()
            .AddApiEndpoints();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });
        
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductsQueryBuilder, ProductsQueryBuilder>();
        
        builder.Services.AddDbContext<ProductsDbContext>(opt => 
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddHealthChecks();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            
            // Run EF migrations when in dev mode
            //app.ApplyMigrations();
        }

        app.MapControllers();
        app.MapHealthChecks("/api/health");
        
        app.UseHttpsRedirection();

        app.MapIdentityApi<User>();
        
        app.UseAuthorization();

        app.Run();
    }
}