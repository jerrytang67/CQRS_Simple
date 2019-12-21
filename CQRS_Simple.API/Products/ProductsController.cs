using System.Threading.Tasks;
using CQRS_Simple.API.Products.Commands;
using CQRS_Simple.API.Products.Queries;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Domain.Products.Request;
using CQRS_Simple.Infrastructure;
using CQRS_Simple.Infrastructure.Uow;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CQRS_Simple.API.Products
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIocManager _iocManager;
        private readonly IRepository<Product, int> _repository;

        public ProductsController(IMediator mediator, IIocManager iocManager, IRepository<Product, int> repository)
        {
            _mediator = mediator;
            _iocManager = iocManager;
            _repository = repository;
        }

        [HttpGet]
        [Route("getProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            // var _repository = _iocManager.GetInstance<IRepository<Product, int>>();

            _repository.UnitOfWork.PrintKey();

            var find = await _repository.GetByIdAsync(12);

            Log.Information(find.Name);
            find.Name += "1";

            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] ProductsRequestInput input)
        {
            var list = await _mediator.Send(new GetProductsQuery(input));
            return Ok(list);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody]Product input)
        {
            var result = await _mediator.Send(new CreateProductCommand(input));
            return result > 0 ? (IActionResult)Ok(result) : BadRequest();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var count = await _mediator.Send(new DeleteProductCommand(id));
            return count > 0 ? (IActionResult)Ok() : NotFound();

        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody]Product input)
        {
            var count = await _mediator.Send(new UpdateProductCommand(input));
            return count > 0 ? (IActionResult)Ok() : NotFound();
        }
    }
}