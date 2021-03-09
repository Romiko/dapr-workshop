using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vigilantes.DaprWorkshop.OrderService.Models
{
    public class CustomerOrder
    {
        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("loyaltyId")]
        public string LoyaltyId { get; set; }

        [JsonProperty("orderItems")]
        public List<CustomerOrderItem> OrderItems { get; set; }
    }
}