using System;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Dapr;
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

        public LoyaltyController(IHttpClientFactory httpClientFactory, ILogger<LoyaltyController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [HttpPost("/orders")]
        [Topic("pubsub", "newOrder")]
        public async Task<IActionResult> UpdateLoyalty(CloudEvent cloudEvent)
        {
            var orderSummary = ((JToken)cloudEvent.Data).ToObject<OrderSummary>();
            _logger.LogInformation("Received Order Summary: {@OrderSummary}", orderSummary);

            // TODO: Challenge 3 - Retrieve and update customer loyalty points
            return Ok();
        }
    }
}
