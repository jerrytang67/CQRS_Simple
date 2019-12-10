using CQRS_Simple.Domain.Products;
using MediatR;

namespace CQRS_Simple.API.Products.Commands
{
    public class UpdateProductCommand : IRequest<int>
    {
        public Product Product { get; set; }
        public UpdateProductCommand(Product product) { Product = product; }
    }
}