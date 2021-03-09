using Newtonsoft.Json;

namespace Vigilantes.DaprWorkshop.VirtualCustomers.Models
{
    public class CustomerOrderItem
    {
        [JsonProperty("menuItemId")]
        public int MenuItemId { get; set; }
        
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        
    }
}