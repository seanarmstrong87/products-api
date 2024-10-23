using ProductsWebApi.Dtos.Requests;
using ProductsWebApi.Services.Models;

namespace ProductsWebApi.Interfaces;

public interface IProductRepository
{
    /// <summary>
    /// Get products based on the filters set in the request.
    /// </summary>
    /// <param name="request">An instance of a <see cref="ProductsRequest"/>.</param>
    /// <returns>An <see cref="ICollection{T}"/> of <see cref="Product"/>.</returns>
    Task<ICollection<Product>> GetProducts(ProductsRequest request);
    
    /// <summary>
    /// Either creates a new product or updates an existing one if a valid productId is provided.
    /// </summary>
    /// <param name="product">The product with the required fields in order to create a new one or update an existing.</param>
    /// <param name="productId">The product ID of the product to be updated or null.</param>
    /// <returns>The <see cref="Product"/> which was created or updated.</returns>
    Task<Product> CreateOrUpdateProduct(Product product, int? productId = null);
}