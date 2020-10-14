using System.Threading;
using System.Threading.Tasks;
using CQRS_Simple.API.Products.Commands;
using CQRS_Simple.Domain.Products;
using CQRS_Simple.Infrastructure.Dapper;
using CQRS_Simple.Infrastructure.MQ;
using MediatR;

namespace CQRS_Simple.API.Products.Handlers
{
    public class UpdateProductHandle : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IDapperRepository<Product, int> _dapperRepository;
        private readonly RabbitMQClient _mq;

        public UpdateProductHandle(IDapperRepository<Product, int> dapperRepository, RabbitMQClient mq)
        {
            _dapperRepository = dapperRepository;
            _mq = mq;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var result = await _dapperRepository.UpdateAsync(request.Product);
            if (result > 0)
                _mq.PushMessage(new RabbitData(typeof(UpdateProductCommand), request));
            return result;
        }
    }
}