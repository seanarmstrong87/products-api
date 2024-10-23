using ProductsWebApi.Dtos.Requests;
using ProductsWebApi.Interfaces;

namespace ProductsWebApi.Services;

internal class ProductsQueryBuilder : IProductsQueryBuilder
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