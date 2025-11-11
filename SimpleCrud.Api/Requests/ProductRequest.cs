using SimpleCrud.Entities;

namespace SimpleCrud.Api.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual Product GetProduct()
        {
            return new Product
            {
                Name = this.Name,
                Description = this.Description,
                Price = this.Price
            };
        }
    }

    public class UpdateProductRequest : ProductRequest
    {
        public int Id { get; set; }

        public override Product GetProduct()
        {
            var result = base.GetProduct();
            result.Id = Id;
            return result;
        }
    }
}
