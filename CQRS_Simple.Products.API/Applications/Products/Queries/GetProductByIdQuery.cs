﻿using CQRS_Simple.API.Products.Dtos;
using MediatR;

namespace CQRS_Simple.Products.API.Applications.Products.Queries
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