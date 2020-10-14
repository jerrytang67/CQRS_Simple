using System.Collections.Generic;
using CQRS_Simple.API.Products.Dtos;
using CQRS_Simple.Domain.Products.Request;
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