using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Dtos;
using CQRS_Simple.Infrastructure;
using CQRS_Simple.MQ;
using CQRS_Simple.Products.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CQRS_Simple.API.Orders
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly RabbitMQClient _mq;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IDapperRepository<Product, int> _productDapperRepository;

        public ProductsController(
            ILogger<ProductsController> logger,
            IMediator mediator,
            IDapperRepository<Product, int> productDapperRepository
            , IMapper mapper, RabbitMQClient mq)
        {
            _logger = logger;
            _mediator = mediator;
            _productDapperRepository = productDapperRepository;
            _mapper = mapper;
            _mq = mq;
        }

        [HttpGet]
        [Route("getProduct/{productId}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            //            var product = await _mediator.Send(new GetProductsQuery(productId));
            var product = await _productDapperRepository.GetByIdAsync(productId);

            product.Code = "asdfsdfasdfasdf";

            await _productDapperRepository.AddAsync(new Product { Code = "aa", Name = "cc" });

            await _productDapperRepository.UpdateAsync(product);

            _logger.LogInformation("GetProduct");

            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        [Route("getAll")]
        [ProducesResponseType(typeof(List<ProductDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(ProductsRequestInput input)
        {
            var list = await _productDapperRepository.FindAsync(x => x.Id > 1);

            _mq.PushMessage(new { TT = 123 });

            return Ok(_mapper.Map<List<ProductDto>>(list.ToList()));
        }

        [HttpDelete]
        [Route("delete/{productId}")]
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