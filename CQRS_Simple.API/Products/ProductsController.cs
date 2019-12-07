using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Dtos;
using CQRS_Simple.Infrastructure;
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
        private readonly IDapperRepository<Product, int> _productDapperRepository;

        public ProductsController(IMediator mediator, IDapperRepository<Product, int> productDapperRepository)
        {
            _mediator = mediator;
            _productDapperRepository = productDapperRepository;
        }

        [HttpGet]
        [Route("getProduct/{productId}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            //            var product = await _mediator.Send(new GetProductsQuery(productId));
            var product = await _productDapperRepository.GetByIdAsync(productId);

            product.Code = "asdfsdfasdfasdf";

            await _productDapperRepository.AddAsync(new Product { Code = "aa", Name = "cc" });

            await _productDapperRepository.UpdateAsync(product);

            return Ok(product);
        }

        [HttpPost]
        [Route("getAll")]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(ProductsRequestInput input)
        {
            var list = await _productDapperRepository.FindAsync(x => x.Id > 1);
            return Ok(list.ToList());
        }

        [HttpDelete]
        [Route("delete/{productId}")]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int productId)
        {
            await _productDapperRepository.RemoveAsync(new Product() { Id = productId });
            return Ok();
        }
    }

    public class ProductsRequestInput
    {

    }
}