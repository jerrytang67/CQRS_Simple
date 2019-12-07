using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace CQRS_Simple.MQ
{
    public class RabbitMQClient : IDisposable
    {
        private readonly RabbitMQOptions _options;

        private IModel _channel;
        private IConnection _connection;

        public RabbitMQClient(
            IOptions<RabbitMQOptions> optionsAccessor
        )
        {
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
                Log.Information($"RabbitMQ Client 连接成功");
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

            Log.Debug($"PushMessage queryName:{queryName} routingKey:{routerKey}");

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
            Log.Information($"RabbitMQ Client Dispose");
        }
    }
}