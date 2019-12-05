using System;
using System.Collections.Generic;
using MediatR;

namespace CQRS_Simple.API.Orders.GetCustomerOrders
{
    public class GetCustomerOrdersQuery : IRequest<List<OrderDto>>
    {
        public Guid CustomerId { get; }

        public GetCustomerOrdersQuery(Guid customerId)
        {
            this.CustomerId = customerId;
        }
    }
}