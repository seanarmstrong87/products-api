using FluentAssertions;
using ProductsWebApi.Dtos.Requests;
using ProductsWebApi.Services;
using ProductsWebApi.Database.Models;

namespace ProductsWebApi.Tests
{
    public class ProductsQueryBuilderTests
    {
        private readonly ProductsQueryBuilder _queryBuilder;

        public ProductsQueryBuilderTests()
        {
            _queryBuilder = new ProductsQueryBuilder();
        }

        [Fact]
        public void GetProductsFromRequest_ShouldFilterByColour()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product1", Colour = ColourChoice.Red },
                new() { Id = 2, Name = "Product2", Colour = ColourChoice.Blue },
                new() { Id = 3, Name = "Product3", Colour = ColourChoice.Green },
                new() { Id = 4, Name = "Product4", Colour = ColourChoice.Black },
            }.AsQueryable();

            var request = new ProductsRequest { Colour = ColourChoice.Red };

            // Act
            var result = _queryBuilder.GetProductsFromRequest(products, request);

            // Assert
            result.Should().HaveCount(1);
            result.First().Colour.Should().Be(ColourChoice.Red);
        }

        [Fact]
        public void GetProductsFromRequest_ShouldFilterByPriceRange()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product1", Price = 10 },
                new() { Id = 2, Name = "Product2", Price = 20 }
            }.AsQueryable();

            var request = new ProductsRequest { Price = new RangeFilter<double> { From = 15, To = 25 } };

            // Act
            var result = _queryBuilder.GetProductsFromRequest(products, request);

            // Assert
            result.Should().HaveCount(1);
            result.First().Price.Should().Be(20);
        }

        [Fact]
        public void GetProductsFromRequest_ShouldFilterByColourAndPriceRange()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product1", Colour = ColourChoice.Red, Price = 10 },
                new() { Id = 2, Name = "Product2", Colour = ColourChoice.Blue, Price = 20 },
                new() { Id = 3, Name = "Product3", Colour = ColourChoice.Red, Price = 20 }
            }.AsQueryable();

            var request = new ProductsRequest { Colour = ColourChoice.Red, Price = new RangeFilter<double> { From = 15, To = 25 } };

            // Act
            var result = _queryBuilder.GetProductsFromRequest(products, request);

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Product3");
        }

        [Fact]
        public void GetProductsFromRequest_ShouldReturnAllProducts_WhenNoFiltersApplied()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product1", Colour = ColourChoice.Red, Price = 10 },
                new() { Id = 2, Name = "Product2", Colour = ColourChoice.Blue, Price = 20 }
            }.AsQueryable();

            var request = new ProductsRequest();

            // Act
            var result = _queryBuilder.GetProductsFromRequest(products, request);

            // Assert
            result.Should().HaveCount(2);
        }
    }
}