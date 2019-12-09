using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CQRS_Simple.API.Products.Commands;
using CQRS_Simple.API.Products.Handlers;
using CQRS_Simple.API.Products.Queries;
using CQRS_Simple.Domain.Products.Request;
using CQRS_Simple.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Simple.API.Products
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) { _mediator = mediator; }

        [HttpGet]
        [Route("getProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return result != null ? (IActionResult) Ok(result) : NotFound();
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] ProductsRequestInput input)
        {
            var list = await _mediator.Send(new GetProductsQuery(input));
            return Ok(list);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
//            await _productDapperRepository.RemoveAsync(new Product() {Id = id});
            return Ok();
        }
    }
}