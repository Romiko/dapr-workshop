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
            
            var pointsEarned = Convert.ToInt32(Math.Ceiling(orderSummary.OrderTotal));
            var loyaltySummary = await _daprClient.GetStateAsync<LoyaltySummary>(StateStore, orderSummary.LoyaltyId);
            if (loyaltySummary == null)
            {
                loyaltySummary = new LoyaltySummary()
                {
                    LoyaltyId = orderSummary.LoyaltyId,
                    CustomerName = orderSummary.CustomerName,
                };
            }

            _logger.LogInformation("Current Points Balance: LoyalityId {0}: {1}", orderSummary.LoyaltyId, loyaltySummary.PointTotal);
            loyaltySummary.PointsEarned = pointsEarned;
            loyaltySummary.PointTotal += loyaltySummary.PointsEarned;
            await _daprClient.SaveStateAsync(StateStore, orderSummary.LoyaltyId, loyaltySummary);
            _logger.LogInformation("New Points Balance: LoyalityId {0}: {1}", orderSummary.LoyaltyId, loyaltySummary.PointTotal);

            return Ok();
        }
    }
}
