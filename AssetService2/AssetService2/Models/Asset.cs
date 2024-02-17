using Newtonsoft.Json;

namespace AssetService2.Models
{
    internal class Asset
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
    }
}
