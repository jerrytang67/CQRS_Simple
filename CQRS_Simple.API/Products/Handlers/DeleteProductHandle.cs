using System.Threading;
using System.Threading.Tasks;
using CQRS_Simple.API.Products.Commands;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Infrastructure.Dapper;
using CQRS_Simple.Infrastructure.MQ;
using MediatR;

namespace CQRS_Simple.API.Products.Handlers
{
    public class DeleteProductHandle : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IDapperRepository<Product, int> _dapperRepository;
        private readonly RabbitMQClient _mq;

        public DeleteProductHandle(IDapperRepository<Product, int> dapperRepository, RabbitMQClient mq)
        {
            _dapperRepository = dapperRepository;
            _mq = mq;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _dapperRepository.RemoveAsync(new Product() {Id = request.ProductId});
            _mq.PushMessage(new RabbitData(typeof(DeleteProductCommand), request));
            return Unit.Value;
        }
    }
}