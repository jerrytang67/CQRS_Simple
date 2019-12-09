using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Dtos;
using CQRS_Simple.Infrastructure.Dapper;
using CQRS_Simple.Infrastructure.MQ;
using CQRS_Simple.Infrastructure.Uow;
using CQRS_Simple.Products.Event;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CQRS_Simple.API.Products
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly RabbitMQClient _mq;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IDapperRepository<Product, int> _productDapperRepository;
        private readonly IRepository<Product, int> _productRepository;

        public ProductsController(
            IMediator mediator,
            IDapperRepository<Product, int> productDapperRepository
            , IMapper mapper, RabbitMQClient mq,
            IRepository<Product, int> productRepository)
        {
            _mediator = mediator;
            _productDapperRepository = productDapperRepository;
            _mapper = mapper;
            _mq = mq;
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("getProduct/{productId}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProduct(int productId)
        {
            //            var product = await _mediator.Send(new GetProductsQuery(productId));

            var product = await _productDapperRepository.GetByIdAsync(productId);

            //            product.Code = "asdfsdfasdfasdf";

            await _productDapperRepository.AddAsync(new Product { Code = "aa", Name = "cc" });

            //            await _productDapperRepository.UpdateAsync(product);

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

        [HttpGet]
        [Route("test")]
        public async Task<string> Test()
        {
            var find = await _productRepository.GetByIdAsync(2);

            _productRepository.UnitOfWork.PrintKey();

            var product = await _mediator.Send(new GetProductsQuery(2));

            return $"{find.Code}";
        }
    }

    public class ProductsRequestInput
    {

    }
}