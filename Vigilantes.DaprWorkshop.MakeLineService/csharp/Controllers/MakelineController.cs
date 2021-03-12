using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Vigilantes.DaprWorkshop.MakeLineService.Models;

namespace Vigilantes.DaprWorkshop.MakeLineService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MakelineController : ControllerBase
    {
        #region  Data members

        private readonly ILogger<MakelineController> _logger;
        private readonly DaprClient _daprClient;
        private readonly HttpClient _httpClient;
        private IServiceManager _serviceManager;

        const string StateStore = "statestore";
        const string StoreId = "Redmond";

        #endregion

        public MakelineController(IHttpClientFactory httpClientFactory,
                                  ILogger<MakelineController> logger, DaprClient daprClient)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpPost("/orders")]
        [Topic("pubsub", "newOrder")]
        public async Task<IActionResult> MakeOrder(CloudEvent cloudEvent)
        {

            // Deserialize incoming order summary
            var orderSummary = ((JToken)cloudEvent.Data).ToObject<OrderSummary>();
            _logger.LogInformation("Received Order: {@OrderSummary}", orderSummary);

            var orders = await _daprClient.GetStateAsync<OrderSummaryUpdateData>(StateStore, orderSummary.StoreId);
            if (orders == null)
            {
                orders = new OrderSummaryUpdateData
                {
                    Arguments = new List<OrderSummary>()
                };
            }

            orders.Target = orderSummary.StoreId;
            orders.Arguments.Add(orderSummary);

            await _daprClient.SaveStateAsync(StateStore, orders.Target, orders);

            // TODO: Challenge 6 - Call the SignalR output binding with the incoming order summary

            return Ok();
        }

        [HttpGet("/orders/{storeId}")]
        public async Task<ActionResult<OrderSummaryUpdateData>> GetOrders(string storeId)
        {
            var orders = await _daprClient.GetStateAsync<OrderSummaryUpdateData>(StateStore, storeId);
            _logger.LogInformation("Get Orders: {@orders}", orders);
            return orders;
        }

        [HttpDelete("/orders/delete/{storeId}/{orderId}")]
        public async Task<IActionResult> DeleteOrder(string storeId, string orderId)
        {
            var orders = await _daprClient.GetStateAsync<OrderSummaryUpdateData>(StateStore, storeId);
            var index = orders.Arguments.FindIndex(o => o.StoreId == storeId && o.OrderId == Guid.Parse(orderId));

            if (index >= 0)
            {
                orders.Arguments.RemoveAt(index);
                _logger.LogInformation("Delete Order: {0}", orderId);
                await _daprClient.SaveStateAsync(StateStore, orders.Target, orders);
            }
             return Ok();
        }


        [EnableCors("CorsPolicy")]
        [HttpPost("/negotiate")]
        public async Task<ActionResult> Negotiate()
        {
            // This method will be called by the signalr client when connecting to
            // the azure signalr service. It will return an access token and the 
            // endpoint details for the client to use when sending and receiving events.     

            return Ok();
            if (_serviceManager == null)
            {
                var connectionString = await GetSignalrConnectionString();
                _serviceManager = new ServiceManagerBuilder()
                .WithOptions(o => o.ConnectionString = connectionString)
                .Build();
            }

            return new JsonResult(new Dictionary<string, string>()
            {
                { "url", _serviceManager.GetClientEndpoint("orders") },
                { "accessToken", _serviceManager.GenerateClientAccessToken("orders", "vigilantes.ui") }
            });
        }

        private async Task<IActionResult> SendOrderUpdate(string eventName, OrderSummary orderSummary)
        {
            // TODO: Challenge 6 - Send event to SignalR output binding
            // References: 
            //    https://github.com/dapr/docs/tree/master/howto/send-events-with-output-bindings
            //    https://github.com/dapr/docs/blob/master/reference/specs/bindings/signalr.md 
            //    Option: use the OrderSummaryUpdateData object
            var message = new OrderSummaryUpdate {
                Data = new OrderSummaryUpdateData {
                    Arguments = new List<OrderSummary> {orderSummary},
                    Target = StoreId
                },

            };
            await  _daprClient.InvokeBindingAsync("signalr", "create", message);
            return new OkResult();
        }

        private async Task<string> GetSignalrConnectionString()
        {
            var secret = await _daprClient.GetSecretAsync("azurekeyvault","signalrConnectionString");
            return secret.Values.First();
        }

    }
}
