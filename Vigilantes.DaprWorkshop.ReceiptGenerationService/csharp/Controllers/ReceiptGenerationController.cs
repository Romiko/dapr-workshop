using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Vigilantes.DaprWorkshop.ReceiptGenerationService.Models;

namespace Vigilantes.DaprWorkshop.ReceiptGenerationService.Controllers
{
    [ApiController]
    public class ReceiptGenerationConsumerController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReceiptGenerationConsumerController> _logger;

        private readonly DaprClient _daprClient;

        public ReceiptGenerationConsumerController(IHttpClientFactory httpClientFactory, ILogger<ReceiptGenerationConsumerController> logger, DaprClient daprClient)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _daprClient = daprClient;
        }

        [Topic("pubsub", "newOrder")]
        [Route("/orders")]
        public async Task<IActionResult> GenerateReceipt(CloudEvent cloudEvent)
        {
            var orderSummary = ((JToken)cloudEvent.Data).ToObject<OrderSummary>();

            _logger.LogInformation("Writing Order Summary (receipt) to storage: {@OrderSummary}", orderSummary);
            await  _daprClient.InvokeBindingAsync("bloborders", "create", orderSummary);
            
            return Ok();
        }
    }
}
