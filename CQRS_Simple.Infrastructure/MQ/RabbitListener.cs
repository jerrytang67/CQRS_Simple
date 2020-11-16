using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CQRS_Simple.Infrastructure.MQ
{
    public class RabbitListener : IHostedService
    {
        private readonly ILogger<RabbitListener> _log;
        private readonly RabbitMQOptions _options;
        private readonly IConnection connection;
        private readonly IModel channel;

        public RabbitListener(
            IOptions<RabbitMQOptions> optionsAccessor,
            IIocManager iocManager
        )
        {
            _log = iocManager.GetInstance<ILogger<RabbitListener>>();
            _options = optionsAccessor.Value;
            try
            {
                var factory = new ConnectionFactory()
                {
                    UserName = _options.UserName,
                    Password = _options.Password,
                    HostName = _options.HostName,
                    Port = _options.Port
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
                _log.LogInformation($"RabbitMQ 连接成功");
            }
            catch (Exception ex)
            {
                _log.LogError($"RabbitListener init error,ex:{ex.Message}");
            }
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Register();
            await Task.CompletedTask;
        }

        protected string QueueName = "QueueName";
        protected string RouteKey;

        // 处理消息的方法
        public virtual async Task Register()
        {
            channel.ExchangeDeclare("message", ExchangeType.Topic, true, false, null);
            channel.QueueDeclare(QueueName, true, false, false, null);

            channel.QueueBind(queue: QueueName, exchange: "message", routingKey: RouteKey);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var result = await ProcessAsync(message);
                _log.LogInformation("收到消息： {@message} routerKey: {@RoutingKey}", message, ea.RoutingKey);
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }

                await Task.Yield();
            };
            channel.BasicConsume(queue: QueueName, consumer: consumer);

            await Task.CompletedTask;
        }

        public virtual Task<bool> ProcessAsync(string message)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            channel?.Dispose();
            connection?.Dispose();
            return Task.CompletedTask;
        }
    }

    public class RabbitMQOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string QueryName { get; set; }
    }
}