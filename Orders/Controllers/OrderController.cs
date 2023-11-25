﻿using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Orders.DataAccess;
using Plain.RabbitMQ;

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDetailsProvider orderDetailsProvider;
        private readonly IPublisher publisher;
        private readonly IOrderCreator orderCreator;

        public OrderController(IOrderDetailsProvider orderDetailsProvider, IPublisher publisher, IOrderCreator orderCreator)
        {
            this.orderDetailsProvider = orderDetailsProvider;
            this.publisher = publisher;
            this.orderCreator = orderCreator;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<OrderDetail> Get()
        {
            return orderDetailsProvider.Get();
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task Post([FromBody] OrderDetail orderDetail)
        {
            var id = await orderCreator.Create(orderDetail);
            publisher.Publish(JsonConvert.SerializeObject(new OrderRequest
            {
                OrderId = id,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity
            }), "order.created", null);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
