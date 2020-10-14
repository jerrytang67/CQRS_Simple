using CQRS_Simple.Domain.Products;

namespace CQRS_Simple.API.Products.Dtos
{
    /// <summary>
    /// <see cref="Product"/>
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }
    }
}