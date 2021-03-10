using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
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
        private readonly DaprClient _daprClient;

        public OrderController(IHttpClientFactory httpClientFactory, ILogger<OrderController> logger, DaprClient daprClient )
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpPost]
        public async Task<IActionResult> NewOrder([FromBody]CustomerOrder order)
        {
            _logger.LogInformation("Customer Order received: {@CustomerOrder}", order);

            // Create an order summary that will be used as the 
            // message body when published
            var orderSummary = CreateOrderSummary(order);
            _logger.LogInformation("Created Order Summary: {@OrderSummary}", orderSummary);

            await _daprClient.PublishEventAsync<OrderSummary>("pubsub", "newOrder", orderSummary);
            return Ok("Publish Success");
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