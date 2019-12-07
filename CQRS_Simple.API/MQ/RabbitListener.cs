using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CQRS_Simple.MQ
{
    public class RabbitListener : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IConnection connection;
        private readonly IModel channel;

        public RabbitListener(
            ILogger<RabbitListener> logger,
            IConfigurationRoot configurationRoot
            )
        {
            _logger = logger;
            _configurationRoot = configurationRoot;
            try
            {
                var factory = new ConnectionFactory()
                {
                    UserName = _configurationRoot.GetSection("RabbitMQ:UserName").ToString(),
                    Password = _configurationRoot.GetSection("RabbitMQ:Password").ToString(),
                    HostName = _configurationRoot.GetSection("RabbitMQ:HostName").ToString(),
                    Port = int.Parse(_configurationRoot.GetSection("RabbitMQ:Port").ToString())
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
                logger.LogWarning($"RabbitMQ 连接成功");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Register();
            await Task.CompletedTask;
        }

        protected string QueueName;
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
                var message = Encoding.UTF8.GetString(body);
                var result = await ProcessAsync(message);
                _logger.LogWarning($"收到消息： {message} routerKey: { ea.RoutingKey}");
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
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

}