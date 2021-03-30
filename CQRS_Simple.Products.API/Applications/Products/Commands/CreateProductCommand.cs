using System.Threading;
using System.Threading.Tasks;
using CQRS_Simple.API.Products.Handlers;
using CQRS_Simple.Core.Dapper;
using CQRS_Simple.Core.MQ;
using CQRS_Simple.Products.API.Domain.Products;
using FluentValidation;
using MediatR;

namespace CQRS_Simple.Products.API.Applications.Products.Commands
{
    public class CreateProductCommandValidate : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidate()
        {
            RuleFor(x => x.Product.Code).Length(10, 256);
        }
    }


    public class CreateProductCommand : IRequest<int>
    {
        public Product Product { get; set; }

        public CreateProductCommand(Product product)
        {
            Product = product;
        }

        public class CreateProductHandle : IRequestHandler<CreateProductCommand, int>
        {
            private readonly IDapperRepository<Product, int> _dapperRepository;
            private readonly RabbitMQClient _mq;

            public CreateProductHandle(RabbitMQClient mq, IDapperRepository<Product, int> dapperRepository)
            {
                _mq = mq;
                _dapperRepository = dapperRepository;
            }

            public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var result = await _dapperRepository.AddAsync(request.Product);
                if (result > 0)
                    _mq.PushMessage(new RabbitData(typeof(CreateProductCommand), request, result));
                return result;
            }
        }
    }
}