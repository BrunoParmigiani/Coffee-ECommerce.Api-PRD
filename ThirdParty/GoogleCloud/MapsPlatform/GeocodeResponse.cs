using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.ThirdParty.GoogleCloud.MapsPlatform
{
    public sealed class GeocodeResponse
    {
        [JsonPropertyName("results")]
        public GeocodeResults[] Results { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
