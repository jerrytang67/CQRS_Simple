using System.Collections.Generic;
using CQRS_Simple.API.Products.Dtos;
using CQRS_Simple.Products.API.Domain.Products.Request;
using MediatR;

namespace CQRS_Simple.Products.API.Applications.Products.Queries
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