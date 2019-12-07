using System.Threading.Tasks;
using CQRS_Simple.MQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CQRS_Simple
{
    public class MyListener : RabbitListener
    {
        public MyListener(IOptions<RabbitMQOptions> optionsAccessor)
            : base(optionsAccessor)
        {
            base.QueueName = "CQRS_Simple_Queue";
            base.RouteKey = "Test.*";
        }

        public override async Task<bool> ProcessAsync(string message)
        {
            return await Task.FromResult(true);
        }
    }
}