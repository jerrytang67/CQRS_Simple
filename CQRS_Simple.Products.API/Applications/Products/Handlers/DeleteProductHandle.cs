using System.Threading;
using System.Threading.Tasks;
using CQRS_Simple.API.Products.Commands;
using CQRS_Simple.API.Products.Handlers;
using CQRS_Simple.Core.Dapper;
using CQRS_Simple.Core.MQ;
using CQRS_Simple.Products.API.Domain.Products;
using MediatR;

namespace CQRS_Simple.Products.API.Applications.Products.Handlers
{
    public class DeleteProductHandle : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IDapperRepository<Product, int> _dapperRepository;
        private readonly RabbitMQClient _mq;

        public DeleteProductHandle(IDapperRepository<Product, int> dapperRepository, RabbitMQClient mq)
        {
            _dapperRepository = dapperRepository;
            _mq = mq;
        }

        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var result = await _dapperRepository.RemoveAsync(new Product() { Id = request.ProductId });
            if (result > 0)
                _mq.PushMessage(new RabbitData(typeof(DeleteProductCommand), request));
            return result;
        }
    }
}