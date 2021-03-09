using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vigilantes.DaprWorkshop.OrderService.Models;

namespace Vigilantes.DaprWorkshop.OrderService.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly ILogger<MenuItemController> _logger;

        public MenuItemController(ILogger<MenuItemController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<MenuItem> Get()
        {
            return MenuItem.GetAll();

        }
    }
}