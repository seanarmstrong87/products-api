using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductsWebApi.Database;
using ProductsWebApi.Dtos.Requests;
using ProductsWebApi.Interfaces;
using ProductsWebApi.Services;
using ProductsWebApi.Services.Models;

namespace ProductsWebApi.Tests
{
    public class ProductRepositoryTests
    {
        private readonly ProductsDbContext _dbContext;
        private readonly ProductRepository _productRepository;
        private Mock<IProductsQueryBuilder> _productsQueryBuilderMock;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new ProductsDbContext(options);
            _productsQueryBuilderMock = new();
            _productRepository = new ProductRepository(_dbContext, _productsQueryBuilderMock.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnProducts()
        {
            // Arrange
            await _dbContext.Database.EnsureDeletedAsync();
            var products = new List<Database.Models.Product>
            {
                new() { Id = 3, Name = "Product 3", Description = "Description three", Price = 10, Colour = ColourChoice.Red },
                new() { Id = 4, Name = "Product 4", Description = "Description four", Price = 20, Colour = ColourChoice.Blue }
            };

            await _dbContext.Products.AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();

            var request = new ProductsRequest();
            _productsQueryBuilderMock.Setup(x =>
                    x.GetProductsFromRequest(It.IsAny<IQueryable<Database.Models.Product>>(),
                        It.IsAny<ProductsRequest>()))
                // Empty request returns all products
                .Returns(_dbContext.Products.AsQueryable()); 

            // Act
            var result = await _productRepository.GetProducts(request);

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Product 3");
        }

        [Fact]
        public async Task CreateOrUpdateProduct_ShouldCreateProduct()
        {
            // Arrange
            var product = new Product("Product 1", "Description one", 10, ColourChoice.Red);

            // Act
            var result = await _productRepository.CreateOrUpdateProduct(product);

            // Assert
            result.Name.Should().Be("Product 1");
            _dbContext.Products.Should().ContainSingle(p => p.Name == "Product 1");
        }

        [Fact]
        public async Task CreateOrUpdateProduct_ShouldUpdateProduct()
        {
            // Arrange
            var existingProduct = new Database.Models.Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10, Colour = ColourChoice.Red };
            await _dbContext.Products.AddAsync(existingProduct);
            await _dbContext.SaveChangesAsync();

            var product = new Product("UpdatedProduct", "UpdatedDescription", 20, ColourChoice.Green);

            // Act
            var result = await _productRepository.CreateOrUpdateProduct(product, 1);

            // Assert
            result.Name.Should().Be("UpdatedProduct");
            existingProduct.Name.Should().Be("UpdatedProduct");
            _dbContext.Products.Should().ContainSingle(p => p.Name == "UpdatedProduct");
        }

        [Fact]
        public async Task CreateOrUpdateProduct_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var nonExistingProductId = 5;
            var product = new Product("UpdatedProduct", "UpdatedDescription", 20, ColourChoice.Green);

            // Act
            Func<Task> act = async () => await _productRepository.CreateOrUpdateProduct(product, nonExistingProductId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage($"Product with Id = {nonExistingProductId} not found");
        }
    }
}