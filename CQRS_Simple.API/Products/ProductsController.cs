using System.Threading.Tasks;
using Autofac;
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
        private readonly ILifetimeScope _container;
        private readonly IIocManager _iocManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IMediator mediator, ILifetimeScope container, IIocManager iocManager, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _container = container;
            _iocManager = iocManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));

            var _r1 = _unitOfWork.GetRepository<Product, int>();

            _r1.UnitOfWork.PrintKey();

            var _repository = _iocManager.GetInstance<IRepository<Product, int>>();
            // var _repository = _container.Resolve<IRepository<Product, int>>();

            _repository.UnitOfWork.PrintKey();

            var _repository2 = _iocManager.GetInstance<IRepository<Product, int>>();
            // var _repository2 = _container.Resolve<IRepository<Product, int>>();

            _repository2.UnitOfWork.PrintKey();

            var find = await _repository.GetByIdAsync(id);

            if (find != null)
            {
                find.Name += "1_";
                Log.Information(find?.Name);
            }

            return result != null ? (IActionResult)Ok(result) : NotFound();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ProductsRequestInput input)
        {
            var list = await _mediator.Send(new GetProductsQuery(input));
            return Ok(list);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody]Product input)
        {
            var result = await _mediator.Send(new CreateProductCommand(input));
            return result > 0 ? (IActionResult)Ok(result) : BadRequest();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var count = await _mediator.Send(new DeleteProductCommand(id));
            return count > 0 ? (IActionResult)Ok() : NotFound();

        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody]Product input)
        {
            var count = await _mediator.Send(new UpdateProductCommand(input));
            return count > 0 ? (IActionResult)Ok() : NotFound();
        }
    }
}