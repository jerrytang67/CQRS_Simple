using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CQRS_Simple.Dtos;
using CQRS_Simple.Products.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Simple.API.Orders
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var orders = await _mediator.Send(new GetProductsQuery(productId));

            return Ok(orders);
        }
    }
}