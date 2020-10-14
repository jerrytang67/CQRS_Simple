using System.Collections.Generic;
using CQRS_Simple.Domain.Products.Request;
using CQRS_Simple.Dtos;
using MediatR;

namespace CQRS_Simple.API.Products.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {
        public ProductsRequestInput Input { get; set; }

        public GetProductsQuery(ProductsRequestInput input)
        {
            Input = input;
        }
    }
}