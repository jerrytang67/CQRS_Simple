using CQRS_Simple.Domain.Products;
using CQRS_Simple.Dtos;
using MediatR;

namespace CQRS_Simple.API.Products.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int ProductId { get; set; }

        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }
    }
}