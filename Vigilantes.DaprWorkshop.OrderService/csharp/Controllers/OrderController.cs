using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vigilantes.DaprWorkshop.OrderService.Models;

namespace Vigilantes.DaprWorkshop.OrderService.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly HttpClient _httpClient;
        public OrderController(IHttpClientFactory httpClientFactory, ILogger<OrderController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> NewOrder([FromBody]CustomerOrder order)
        {
            _logger.LogInformation("Customer Order received: {@CustomerOrder}", order);

            // Create an order summary that will be used as the 
            // message body when published
            var orderSummary = CreateOrderSummary(order);
            _logger.LogInformation("Created Order Summary: {@OrderSummary}", orderSummary);

            // TODO: Challenge 2 - Publish an OrderSummary message via Dapr
            return Ok("Bummer. Business logic and pub/sub isn't implemented yet but, hey, at least your POST worked and you should see the order in the log! YOINK!");
        }

        private static OrderSummary CreateOrderSummary(CustomerOrder order)
        {
            // Retrieve all the menu items
            var menuItems = MenuItem.GetAll();

            // Iterate through the list of ordered items to calculate
            // the total and compile a list of item summaries.
            var orderTotal = 0.0m;
            var itemSummaries = new List<OrderItemSummary>();
            foreach (var orderItem in order.OrderItems)
            {
                var menuItem = menuItems.FirstOrDefault(x => x.MenuItemId == orderItem.MenuItemId);
                if (menuItem == null) continue;

                orderTotal += (menuItem.Price * orderItem.Quantity);
                itemSummaries.Add(new OrderItemSummary
                {
                    Quantity = orderItem.Quantity,
                    MenuItemId = orderItem.MenuItemId,
                    MenuItemName = menuItem.Name
                });
            }

            // Initialize and return the order summary
            var summary = new OrderSummary
            {
                OrderId = Guid.NewGuid(),
                StoreId = order.StoreId,
                CustomerName = order.CustomerName,
                LoyaltyId = order.LoyaltyId,
                OrderDate = DateTime.UtcNow,
                OrderItems = itemSummaries,
                OrderTotal = orderTotal
            };

            return summary;
        }
    }
}