using System.Threading.Tasks;
using CQRS_Simple.Infrastructure.MQ;
using Microsoft.Extensions.Options;

namespace CQRS_Simple
{
    public class MyListener : RabbitListener
    {
        private readonly RabbitMQOptions _options;

        public MyListener(IOptions<RabbitMQOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            _options = optionsAccessor.Value;
            base.QueueName = _options.QueryName;
            base.RouteKey = "Test.*";
        }

        public override async Task<bool> ProcessAsync(string message)
        {
            return await Task.FromResult(true);
        }
    }
}