﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersApp.Data;
using OrdersApp.Models;

namespace OrdersApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrdersAppDbContext dbContext;

        public OrderController(OrdersAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            return Ok(await dbContext.Orders.ToListAsync());
        }

        [HttpGet]
        [Route("id:guid")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id)
        {
            var order=await dbContext.Orders.FindAsync(id);
            if(order == null)
            {
                return NotFound();

            }
            return Ok(order);
        }

        [HttpDelete]
        [Route("id:guid")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id)
        {
            var order= await dbContext.Orders.FindAsync(id);
            if (order != null)
            {
                dbContext.Remove(order);
                await dbContext.SaveChangesAsync();
                return Ok(order);
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrder(OrderRequests orderRequests)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                Price = orderRequests.Price,
                CreatedDate = orderRequests.CreatedDate,
                OrderName = orderRequests.OrderName
            };

          await  dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();

            return Ok(order);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, UpdateOrderRequest updateOrderRequest)
        {
           var order= await dbContext.Orders.FindAsync(id);

            if(order!=null)
            {
                order.Price = updateOrderRequest.Price;
                order.CreatedDate = updateOrderRequest.CreatedDate;
                order.OrderName=updateOrderRequest.OrderName;

                await dbContext.SaveChangesAsync();

                return Ok(order);
            }



            return NotFound();
        }
    }
}
