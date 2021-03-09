using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vigilantes.DaprWorkshop.MakeLineService.Models
{
    public class OrderSummaryUpdate
    {
        [JsonProperty("data")]
        public OrderSummaryUpdateData Data { get; set; }

        [JsonProperty("metadata")]
        public object Metadata { get; set; }
        
    }
}