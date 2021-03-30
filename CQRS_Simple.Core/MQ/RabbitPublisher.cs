using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CQRS_Simple.Core.MQ
{
    public class RabbitMQClient : IDisposable
    {
        private readonly ILogger<RabbitMQClient> _log;
        private readonly RabbitMQOptions _options;

        private IModel _channel;
        private IConnection _connection;

        public RabbitMQClient(
            IOptions<RabbitMQOptions> optionsAccessor,
            ILogger<RabbitMQClient> log
        )
        {
            _log = log;
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
                this._connection = factory.CreateConnection();
                this._channel = _connection.CreateModel();
                _log.LogInformation($"RabbitMQ Client 连接成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }

        public virtual void PushMessage(object message, string queryName = null, string routerKey = "Test.*")
        {
            if (queryName == null)
                queryName = _options.QueryName;

            var exchangeName = "message";

            _log.LogDebug("PushMessage queryName:{@queryName} routingKey:{@routerKey}", queryName, routerKey);

            //定义一个Direct类型交换机
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true, false, null);

            //定义一个队列
            _channel.QueueDeclare(queryName, true, false, false, null);

            //将队列绑定到交换机
            _channel.QueueBind(queryName, exchangeName, routerKey, null);

            var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            _channel.BasicPublish(exchangeName, routerKey, null, sendBytes);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            _log.LogInformation($"RabbitMQ Client Dispose");
        }
    }
}