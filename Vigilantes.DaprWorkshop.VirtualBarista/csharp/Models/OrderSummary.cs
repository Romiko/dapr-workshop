using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vigilantes.DaprWorkshop.VirtualBarista.Models
{
    public class OrderSummary
    {
        [JsonProperty("orderId")]
        public Guid OrderId { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("storeId")]
        public string StoreId { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("loyaltyId")]
        public string LoyaltyId { get; set; }

        [JsonProperty("orderItems")]
        public List<OrderItemSummary> OrderItems { get; set; }

        [JsonProperty("orderTotal")]
        public decimal OrderTotal { get; set; }
    }
}