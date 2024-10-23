using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsWebApi.Dtos;
using ProductsWebApi.Dtos.Requests;
using ProductsWebApi.Interfaces;
using ProductsWebApi.Mapper;

namespace ProductsWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ProductMapper _productMapper;
    
    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _productMapper = new ProductMapper();
    }

    [HttpGet]
    public async Task<ICollection<GetProductDto>> GetProducts([FromQuery] ProductsRequest request)
    {
        var products = await _productRepository.GetProducts(request);
        
        return products.Select(_productMapper.ProductToDto).ToList();
    }

    [HttpPost]
    public async Task<GetProductDto> CreateProduct([Required] [FromBody] PostProductDto getProductDtoDto)
    {
        var createdProduct = await _productRepository.CreateOrUpdateProduct(_productMapper.DtoToProduct(getProductDtoDto));
        
        return _productMapper.ProductToDto(createdProduct);
    }
    
    [HttpPut("{productId}")]
    public async Task<GetProductDto> CreateProduct([Required][FromRoute] int productId, [Required] [FromBody] PostProductDto getProductDtoDto)
    {
        var createdProduct = await _productRepository.CreateOrUpdateProduct(_productMapper.DtoToProduct(getProductDtoDto), productId);
        
        return _productMapper.ProductToDto(createdProduct);
    }

}