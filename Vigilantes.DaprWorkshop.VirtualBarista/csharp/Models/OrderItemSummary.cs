using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vigilantes.DaprWorkshop.VirtualBarista.Models
{
    public class OrderItemSummary
    {
        [JsonProperty("menuItemId")]
        public int MenuItemId { get; set; }

        [JsonProperty("menuItemName")]
        public string MenuItemName { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

        public class OrderSummaryDto
    {
        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("arguments")]
        public List<OrderSummary> Arguments { get; set; }        
    }
}