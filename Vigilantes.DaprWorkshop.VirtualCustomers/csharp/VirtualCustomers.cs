using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vigilantes.Dapr.Extensions;
using Vigilantes.DaprWorkshop.VirtualCustomers.Models;

namespace Vigilantes.DaprWorkshop.VirtualCustomers
{
    public class VirtualCustomers : BackgroundService
    {
        private const string OrderServiceDaprId = "order-service";
        private const string StoreId = "Redmond";
        public static readonly int maxItemQuantity = int.Parse(Environment.GetEnvironmentVariable("MAX_ITEM_QUANTITY") ?? "5");
        public static readonly int drinkCadenceSeconds = int.Parse(Environment.GetEnvironmentVariable("DRINK_CADENCE_SEC") ?? "5");
        public static readonly int numDrinkOrders = int.Parse(Environment.GetEnvironmentVariable("NUM_DRINK_ORDERS") ?? "10");

        private readonly HttpClient _httpClient;
        private readonly ILogger<VirtualCustomers> _logger;
        private Task _ordersTask = Task.CompletedTask;

        private static readonly (int, string)[] customers = {
            (1, "Bruce Wayne"),
            (2, "Britt Reid"),
            (3, "Robin Hood"),
            (4, "Don Diego de la Vega"),
            (5, "Barry Allen"),
            (6, "Michael Knight"),
            (7, "Angus MacGyver"),
            (8, "Robert McCall"),
            (9, "John 'Hannibal' Smith"),
            (10, "H.M. 'Howlin Mad' Murdock"),
            (11, "B. A. Baracus"),
            (12, "Templeton 'Faceman' Peck"),
            (13, "Peter Parker"),
            (14, "Leonardo"),
            (15, "Donatello"),
            (16, "Raphael"),
            (17, "Michelangelo"),
            (18, "Frank Castle"),
            (19, "Matt Murdock"),
            (20, "Harry Callahan")
        };

        public VirtualCustomers(IHttpClientFactory httpClientFactory, ILogger<VirtualCustomers> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"The customers are ready to order their drinks!");

            stoppingToken.Register(() =>
            {
                _ordersTask.Wait();
                _logger.LogInformation($"The remaining customers have cancelled their orders and are leaving the store.");
                Environment.Exit(1);
            });

            int ordersCreated = 0;
            await Task.Delay(3000);
            do
            {
                await CreateOrder(stoppingToken);
                ordersCreated += 1;

            } while (!stoppingToken.IsCancellationRequested && (ordersCreated < numDrinkOrders || numDrinkOrders == -1));

            _logger.LogInformation($"The {(numDrinkOrders == 1 ? "customer has" : "customers have")} finished placing {numDrinkOrders} {(numDrinkOrders == 1 ? "order" : "orders")}.");
        }

        public async Task CreateOrder(CancellationToken stoppingToken)
        {
            CustomerOrder order = new CustomerOrder();
            Random rnd = new Random();
            int custNum = rnd.Next(0, customers.Length);
            _logger.LogInformation("Customer {0} with loyalty id {1} is placing an order.", customers[custNum].Item2, customers[custNum].Item1);
            order.StoreId = StoreId;
            order.CustomerName = customers[custNum].Item2;
            order.LoyaltyId = customers[custNum].Item1.ToString();

            // Get total number of menu items (t) and ramdomly choose a number of them to order (n)
            var menuItems = MenuItem.GetAll();
            int menuItemTotal = menuItems.Count;
            int menuItemNum = rnd.Next(1, menuItemTotal + 1); // Never order 0 items

            // Randomly choose a number between (1-t), (n) times to get a random quantity of (n) items
            List<CustomerOrderItem> orderItems = new List<CustomerOrderItem>();
            List<int> menuIndicies = new List<int>();
            for (int i = 0; i < menuItemNum; i++)
            {
                int menuIndex = rnd.Next(1, menuItemTotal + 1);
                while (menuIndicies.Contains(menuIndex)) // Ensure no duplicate menu items within order
                {
                    menuIndex = rnd.Next(1, menuItemTotal + 1);
                }

                menuIndicies.Add(menuIndex);
                var menuItem = menuItems.Find(x => x.MenuItemId == menuIndex); // Assuming menuIds are numbered #1 to #menuItemTotal
                int quantity = rnd.Next(1, maxItemQuantity + 1);

                orderItems.Add(new CustomerOrderItem
                {
                    MenuItemId = menuIndex,
                    Quantity = quantity
                });
            }

            order.OrderItems = orderItems;
            if (!stoppingToken.IsCancellationRequested)
            {
                HttpResponseMessage response = await order.DaprHttpInvokePostAsync(OrderServiceDaprId, $"order/", _httpClient, stoppingToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Order was unsuccessful: {0} {1} {2}", (int)response.StatusCode, response.StatusCode, await response.Content.ReadAsStringAsync());
                }
                else
                {
                    _ordersTask = Task.Delay(drinkCadenceSeconds * 1000, stoppingToken); // Delay until next customer in the line id ready to order
                    await _ordersTask;
                    _logger.LogInformation("Customer {0} has placed their order containing {1} items.", order.CustomerName, order.OrderItems.Count);
                }
            }
        }
    }
}