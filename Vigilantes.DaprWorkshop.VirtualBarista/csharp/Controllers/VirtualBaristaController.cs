using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vigilantes.DaprWorkshop.VirtualBarista.Models;
namespace Vigilantes.DaprWorkshop.VirtualBarista.Controllers
{
    [ApiController]
    public class VirtualBaristaController : ControllerBase
    {
        public static readonly string StoreId = Environment.GetEnvironmentVariable("STORE_ID") ?? "Redmond";
        public static readonly int MinSecondsToMakeDrink = int.Parse(Environment.GetEnvironmentVariable("MIN_SECONDS_TO_MAKE_DRINK") ?? "3");
        public static readonly int MaxSecondsToMakeDrink = int.Parse(Environment.GetEnvironmentVariable("MAX_SECONDS_TO_MAKE_DRINK") ?? "10");
        private static readonly object DrinkLock = new object();
        private static bool IsMakingDrinks = false;
        private readonly HttpClient _httpClient;
        private readonly ILogger<VirtualBaristaController> _logger;
        private readonly Random _random;

        public VirtualBaristaController(IHttpClientFactory httpClientFactory, ILogger<VirtualBaristaController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _random = new Random();
        }

        [HttpPost]
        [Route("/orders")]
        public async Task<IActionResult> CheckOrders()
        {
            if (!IsMakingDrinks)
            {
                _logger.LogInformation($"The VirtualBarista ({StoreId}) is checking orders on the make line...");

                var orders = await GetOrders();

                _logger.LogInformation($"There {(orders.Count == 1 ? "is" : "are")} {orders.Count} {(orders.Count == 1 ? "order" : "orders")} waiting to be made.");

                if (orders.Count > 0)
                {
                    lock (DrinkLock)
                    {
                        IsMakingDrinks = true;
                    }

                    try
                    {
                        while (orders.Count > 0)
                        {
                            var order = orders.First();

                            _logger.LogInformation($"The VirtualBarista ({StoreId}) is making an order for {order.CustomerName}...");

                            foreach (var orderItem in order.OrderItems)
                            {
                                _logger.LogInformation($"The VirtualBarista ({StoreId}) is making {orderItem.Quantity} {orderItem.MenuItemName}.");

                                await Task.Delay(_random.Next(MinSecondsToMakeDrink * 1000, MaxSecondsToMakeDrink * 1000));

                                _logger.LogInformation($"The VirtualBarista ({StoreId}) completed {orderItem.Quantity} {orderItem.MenuItemName}.");
                            }

                            await CompleteOrder(order);
                            orders.RemoveAt(0);

                            _logger.LogInformation($"{order.CustomerName}, your drinks are ready!");
                        }
                    }
                    finally
                    {
                        lock (DrinkLock)
                        {
                            IsMakingDrinks = false;
                        }
                    }
                }
                else
                {
                    _logger.LogInformation($"The make line is empty! Time to drum up some customers!");

                    await Task.Delay(5000);
                }
            }

            return Ok();
        }

        private async Task<List<OrderSummary>> GetOrders()
        {
            // TODO: Challenge 4 - Call the Makeline service via Dapr to get orders
            await Task.Delay(5000);
            return Enumerable.Empty<OrderSummary>().ToList();
        }

        private async Task CompleteOrder(OrderSummary orderSummary)
        {
            // TODO: Challenge 4 - Call the Makeline service via Dapr to complete the order
            await Task.Delay(5000);
        }
    }
}