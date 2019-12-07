using System.ComponentModel.DataAnnotations;

namespace CQRS_Simple.Domain.Products
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [StringLength(256)]
        [Required]
        public string Name { get; set; }

        [StringLength(256)]
        public string Code { get; set; }

        public string Description { get; set; }
    }
}