using System;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Vigilantes.DaprWorkshop.LoyaltyService.Models;

namespace Vigilantes.DaprWorkshop.LoyaltyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoyaltyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoyaltyController> _logger;

        private readonly DaprClient _daprClient;

        const string StateStore = "statestore";

        public LoyaltyController(IHttpClientFactory httpClientFactory, ILogger<LoyaltyController> logger, DaprClient daprClient)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpPost("/orders")]
        [Topic("pubsub", "newOrder")]
        public async Task<IActionResult> UpdateLoyalty(CloudEvent cloudEvent)
        {
            var orderSummary = ((JToken)cloudEvent.Data).ToObject<OrderSummary>();
            _logger.LogInformation("Received Order Summary: {@OrderSummary}", orderSummary);

            var customerLoyalityPoints = await _daprClient.GetStateAsync<int>(StateStore, orderSummary.LoyaltyId);
            _logger.LogInformation("Current Points Balance: LoyalityId {0}: {1}", orderSummary.LoyaltyId, customerLoyalityPoints);
            var newPoints = Math.Ceiling(orderSummary.OrderTotal);
            customerLoyalityPoints = customerLoyalityPoints + Convert.ToInt32(newPoints);
            await _daprClient.SaveStateAsync(StateStore, orderSummary.LoyaltyId, customerLoyalityPoints);
            _logger.LogInformation("New Points Balance: LoyalityId {0}: {1}", orderSummary.LoyaltyId, customerLoyalityPoints);

            return Ok();
        }
    }
}
