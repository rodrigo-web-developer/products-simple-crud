using SimpleCrud.Validations;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrud.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [PositiveNumber]
        public decimal Price { get; set; }

    }
}
