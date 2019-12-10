using CQRS_Simple.Domain.Products;
using MediatR;

namespace CQRS_Simple.API.Products.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public Product Product { get; set; }
        public CreateProductCommand(Product product)
        {
            Product = product;
        }
    }
}