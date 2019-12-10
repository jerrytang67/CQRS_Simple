using System;

namespace CQRS_Simple.API.Products.Handlers
{
    public class RabbitData
    {
        public string Type { get; private set; }
        public object Request { get; private set; }

        public object Result { get; private set; }

        public RabbitData(Type type, object request, object result = null)
        {
            Type = $"{type}";
            Request = request;
            Result = result;
        }
    }
}