using CQRS_Simple.Products.API.Domain.Products;
using MediatR;

namespace CQRS_Simple.Products.API.Applications.Products.Commands
{
    public class UpdateProductCommand : IRequest<int>
    {
        public Product Product { get; set; }
        public UpdateProductCommand(Product product) { Product = product; }
    }
}