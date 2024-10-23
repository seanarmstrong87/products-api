using ProductsWebApi.Dtos;
using ProductsWebApi.Services.Models;
using Riok.Mapperly.Abstractions;

namespace ProductsWebApi.Mapper;

[Mapper]
public partial class ProductMapper
{
    public partial GetProductDto ProductToDto(Product product);
    public partial Product DtoToProduct(PostProductDto getProductDto);
    public partial Product DtoToProduct(GetProductDto getProductDto);
}