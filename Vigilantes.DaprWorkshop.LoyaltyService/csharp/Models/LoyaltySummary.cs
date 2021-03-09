using Newtonsoft.Json;

namespace Vigilantes.DaprWorkshop.LoyaltyService.Models
{
    public class LoyaltySummary
    {
        [JsonProperty("loyaltyId")]
        public string LoyaltyId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("pointsEarned")]
        public int PointsEarned { get; set; }
        [JsonProperty("pointTotal")]
        public int PointTotal { get; set; }
    }
}