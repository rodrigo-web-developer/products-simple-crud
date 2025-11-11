using Microsoft.AspNetCore.Mvc;
using SimpleCrud.Api.Controllers;
using SimpleCrud.Api.Requests;
using SimpleCrud.Entities;
using SimpleCrud.Repositories.Impl;
using SimpleCrud.Services;

namespace SimpleCrud.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly ProductService service;
        private readonly ProductsController _controller;

        public ProductControllerTests()
        {
            // testing directly implementation
            service = new ProductService(new InMemoryRepository());
            _controller = new ProductsController(service);
        }

        #region GetById Tests

        [Fact]
        public async Task GetById_WhenProductExists_ReturnsOkWithProduct()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 10.99m };
            var result = Result<Product>.Ok(product);

            // Act
            var res = await service.CreateAsync(product);
            var productId = res.Data.Id;

            var actionResult = await _controller.GetById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productId, returnedProduct.Id);
            Assert.Equal("Test Product", returnedProduct.Name);
        }

        [Fact]
        public async Task GetById_WhenProductDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var productId = 999;
            var result = Result<Product>.Fail("Product not found");

            // Act
            var actionResult = await _controller.GetById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }

        #endregion

        #region GetAll Tests

        [Fact]
        public async Task GetAll_ReturnsOkWithProductList()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Name = "Product 1", Price = 10m },
                new Product { Name = "Product 2", Price = 20m }
            };
            var result = Result<List<Product>>.Ok(products);
            // make it valid for concurrence
            var oldList = await service.GetAll();

            var oldCount = oldList.Data.Count;

            // Act
            foreach (var prod in products)
            {
                await service.CreateAsync(prod);
            }

            var actionResult = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(oldCount + 2, returnedProducts.Count);
        }

        [Fact]
        public async Task GetAll_WhenNoProducts_ReturnsEmptyList()
        {
            // Arrange
            var result = Result<List<Product>>.Ok(new List<Product>());

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var returnedProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Empty(returnedProducts);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task Create_WithValidProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var request = new ProductRequest { Name = "New Product", Description = "Description", Price = 15.99m };
            var product = new Product { Id = 1, Name = "New Product", Description = "Description", Price = 15.99m };
            var result = Result<Product>.Ok(product, "Product created successfully.");

            // Act
            var actionResult = await _controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Equal(nameof(ProductsController.GetById), createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues["id"]);
            var returnedProduct = Assert.IsType<Product>(createdResult.Value);
            Assert.Equal("New Product", returnedProduct.Name);
        }

        [Fact]
        public async Task Create_WithInvalidProduct_ReturnsUnprocessableEntity()
        {
            // Arrange
            var request = new ProductRequest { Name = "", Price = -5m };
            var errors = new[] { "Name is required", "Price must be positive" };
            var result = Result<Product>.Fail(errors, "Validation failed.");

            // Act
            var actionResult = await _controller.Create(request);

            // Assert
            var unprocessableResult = Assert.IsType<UnprocessableEntityObjectResult>(actionResult);
            var returnedErrors = Assert.IsAssignableFrom<IEnumerable<string>>(unprocessableResult.Value);
            Assert.Equal(2, returnedErrors.Count());
        }

        #endregion
    }
}
