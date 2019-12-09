using System;

namespace CQRS_Simple.API.Products.Handlers
{
    public class RabbitData
    {
        public string Type { get; private set; }
        public Object Request { get; private set; }

        public RabbitData(Type type, Object request)
        {
            Type = $"{type}";
            Request = request;
        }
    }
}