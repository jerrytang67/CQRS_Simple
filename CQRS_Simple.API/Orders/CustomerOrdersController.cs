using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CQRS_Simple.API.Orders.GetCustomerOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Simple.API.Orders
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Get customer orders.
        /// </summary>
        /// <param name="customerId">Customer ID.</param>
        /// <returns>List of customer orders.</returns>
        [Route("{customerId}/orders")]
        [HttpGet]
        [ProducesResponseType(typeof(List<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerOrders(Guid customerId)
        {
            var orders = await _mediator.Send(new GetCustomerOrdersQuery(customerId));

            return Ok(orders);
        }
    }
}