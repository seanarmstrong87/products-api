using ProductsWebApi.Dtos.Requests;
using ProductsWebApi.Interfaces;

namespace ProductsWebApi.Services;

/// <summary>
/// A class for building queries for products based on a <see cref="ProductsRequest"/>.
///
/// This keeps the logic for building queries in one place separate from the repository and allows for easier testing.
/// </summary>
public class ProductsQueryBuilder : IProductsQueryBuilder
{
    public IQueryable<Database.Models.Product> GetProductsFromRequest(
        IQueryable<Database.Models.Product> query, ProductsRequest request)
    {
        if (request.Colour is not null)
        {
            query = query.Where(p => p.Colour == request.Colour);
        }

        if (request.Price?.From is not null)
        {
            query = query.Where(p => p.Price >= request.Price.From);
        }

        if (request.Price?.To is not null)
        {
            query = query.Where(p => p.Price <= request.Price.To);
        }

        return query;
    }
}