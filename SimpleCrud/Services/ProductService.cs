using SimpleCrud.Entities;
using SimpleCrud.Extensions;
using SimpleCrud.Repositories;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrud.Services
{
    public class ProductService
    {
        private readonly IRepository repository;

        public ProductService(IRepository repository)
        {
            this.repository = repository;
        }

        public virtual async Task<Result<Product>> CreateAsync(Product product)
        {
            if (product is null) return Result<Product>.Fail("Product cannot be null.");

            var errors = Validate(product).ToArray();
            if (errors.Length > 0) return Result<Product>.Fail(errors, "Validation failed.");

            try
            {
                await repository.AddAsync(product);
                return Result<Product>.Ok(product, "Product created successfully.");
            }
            catch (System.Exception ex)
            {
                return Result<Product>.Fail(ex.Message, "An error occurred while creating the product.");
            }
        }

        public virtual async Task<Result<Product>> UpdateAsync(Product product)
        {
            if (product is null) return Result<Product>.Fail("Product cannot be null.");
            if (product.Id <= 0) return Result<Product>.Fail("Product must have a valid Id to update.");

            try
            {
                var existing = await repository.FindByIdAsync<Product>(product.Id);
                if (existing is null) return Result<Product>.Fail($"Product with Id {product.Id} not found.");

                var errors = Validate(product).ToArray();
                if (errors.Any()) return Result<Product>.Fail(errors, "Validation failed.");

                await repository.UpdateAsync(product);
                return Result<Product>.Ok(product, "Product updated successfully.");
            }
            catch (System.Exception ex)
            {
                return Result<Product>.Fail(ex.Message, "An error occurred while updating the product.");
            }
        }

        public virtual async Task<Result> DeleteAsync(int id)
        {
            if (id <= 0) return Result.Fail("Invalid id.");

            try
            {
                var existing = await repository.FindByIdAsync<Product>(id);
                if (existing is null) return Result.Fail($"Product with Id {id} not found.");

                var deleted = await repository.DeleteAsync(existing);
                if (deleted > 0) return Result.Ok("Product deleted.");

                return Result.Fail("Could not delete product.");
            }
            catch (System.Exception ex)
            {
                return Result.Fail(ex.Message, "An error occurred while deleting the product.");
            }
        }

        public virtual async Task<Result<Product>> GetByIdAsync(int id)
        {
            if (id <= 0) return Result<Product>.Fail("Invalid id.");

            var product = await repository.FindByIdAsync<Product>(id);
            if (product is null) return Result<Product>.Fail($"Product with Id {id} not found.");

            return Result<Product>.Ok(product);
        }

        public virtual async Task<Result<List<Product>>> GetAll()
        {
            var result = await repository.Query<Product>().ToListAsync();
            return Result<List<Product>>.Ok(result);
        }

        protected virtual IEnumerable<string> Validate<T>(T entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, results, validateAllProperties: true);
            return results.Select(r => r.ErrorMessage ?? r.ToString());
        }
    }
}
