using System.Threading;
using System.Threading.Tasks;
using CQRS_Simple.API.Products.Commands;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Infrastructure.Dapper;
using CQRS_Simple.Infrastructure.MQ;
using MediatR;

namespace CQRS_Simple.API.Products.Handlers
{
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