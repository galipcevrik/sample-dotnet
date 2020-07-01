using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Models
{
    public class ProductPrice
    {
        [JsonProperty("rate_of_exchange")]
        public double RateOfExchange { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

    }
}
