using ProductsWebApi.Database;
using ProductsWebApi.Dtos.Requests;
using ProductsWebApi.Interfaces;
using ProductsWebApi.Services.Models;

namespace ProductsWebApi.Services;

public class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _dbContext;
    private readonly IProductsQueryBuilder _productsQueryBuilder;
    
    public ProductRepository(ProductsDbContext dbContext, IProductsQueryBuilder productsQueryBuilder)
    {
        _dbContext = dbContext;
        _productsQueryBuilder = productsQueryBuilder;
    }
    
    public async Task<ICollection<Product>> GetProducts(ProductsRequest request)
    {
        var query = _dbContext.Products.AsQueryable();
        
        query = _productsQueryBuilder.GetProductsFromRequest(query, request);
        
        return query.AsEnumerable().Select(ToServiceProduct).ToList();
    }

    public async Task<Product> CreateOrUpdateProduct(Product product, int? productId = null)
    {
        if(productId is not null)
        {
            var existingProduct = await _dbContext.Products.FindAsync(productId);
            
            if(existingProduct is null)
            {
                throw new ArgumentException($"Product with Id = {productId} not found");
            }
            
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Colour = product.Colour;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync();
            
            return ToServiceProduct(existingProduct);
        }
        
        var createdEntity = await _dbContext.Products.AddAsync(new Database.Models.Product
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Colour = product.Colour,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
        
        await _dbContext.SaveChangesAsync();
        
        return ToServiceProduct(createdEntity.Entity);
    }

    private static Product ToServiceProduct(Database.Models.Product dbProduct)
    {
        return new Product(dbProduct.Name, dbProduct.Description, dbProduct.Price, dbProduct.Colour)
        {
            CreatedAt = dbProduct.CreatedAt,
            UpdatedAt = dbProduct.UpdatedAt,
            Id = dbProduct.Id,
        };
    }
}