using ProductsWebApi.Dtos.Requests;

namespace ProductsWebApi.Interfaces;

public interface IProductsQueryBuilder
{
    IQueryable<Database.Models.Product> GetProductsFromRequest(IQueryable<Database.Models.Product> query,
        ProductsRequest request);
}