using System.Threading.Tasks;
using CQRS_Simple.Core;
using CQRS_Simple.Core.MQ;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CQRS_Simple.Products.API
{
    public class MyListener : RabbitListener
    {
        private readonly ILogger<MyListener> _logger;
        private readonly RabbitMQOptions _options;

        public MyListener(
            IOptions<RabbitMQOptions> optionsAccessor,
            IIocManager iocManager,
            ILogger<MyListener> logger
        )
            : base(optionsAccessor, iocManager, logger)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
            QueueName = _options.QueryName;
            RouteKey = "Test.*";
        }

        public override async Task<bool> ProcessAsync(string message)
        {
            return await Task.FromResult(true);
        }
    }
}