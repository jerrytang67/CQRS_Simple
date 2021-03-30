using System.Diagnostics;
using System.Threading.Tasks;
using CQRS_Simple.API.Applications.Products.Queries;
using CQRS_Simple.API.Products.Commands;
using CQRS_Simple.Core;
using CQRS_Simple.Core.Uow;
using CQRS_Simple.Products.API.Applications.Products.Commands;
using CQRS_Simple.Products.API.Domain.Products;
using CQRS_Simple.Products.API.Domain.Products.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CQRS_Simple.API.Products
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMediator _mediator;
        private readonly IIocManager _iocManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(
            ILogger<ProductsController> logger,
            IMediator mediator,
            IIocManager iocManager,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mediator = mediator;
            _iocManager = iocManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));

            var _r1 = _unitOfWork.GetRepository<Product, int>();
            _r1.UnitOfWork.PrintKey();


            var _repository = _iocManager.GetInstance<IRepository<Product, int>>();
            _repository.UnitOfWork.PrintKey();


            var _repository2 = _iocManager.GetInstance<IRepository<Product, int>>();
            _repository2.UnitOfWork.PrintKey();


            var find = await _repository.GetByIdAsync(id);

            if (find != null)
            {
                find.Name += "1_";
                Log.Information(find?.Name);
                await _unitOfWork.SaveChangesAsync();
            }

            // throw new Exception("ss");

            return result != null ? (IActionResult) Ok(result) : NotFound();
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ProductsRequestInput input)
        {
            var list = await _mediator.Send(new GetProductsQuery(input));

            Debugger.Break();

            return Ok(list);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Product input)
        {
            var result = await _mediator.Send(new CreateProductCommand(input));
            return result > 0 ? (IActionResult) Ok(result) : BadRequest();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var count = await _mediator.Send(new DeleteProductCommand(id));
            return count > 0 ? (IActionResult) Ok() : NotFound();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Product input)
        {
            var count = await _mediator.Send(new UpdateProductCommand(input));
            return count > 0 ? (IActionResult) Ok() : NotFound();
        }
    }
}