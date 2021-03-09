using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Vigilantes.DaprWorkshop.ReceiptGenerationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DaprController : ControllerBase
    {
        private readonly ILogger<DaprController> _logger;

        public DaprController(ILogger<DaprController> logger)
        {
            _logger = logger;
        }

        // TODO: Challenge 2
    }
}
