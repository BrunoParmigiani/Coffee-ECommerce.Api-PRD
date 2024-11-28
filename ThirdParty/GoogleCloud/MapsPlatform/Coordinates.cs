using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.ThirdParty.GoogleCloud.MapsPlatform
{
    public sealed class Coordinates
    {
        [JsonPropertyName("lat")]
        public float Latitude { get; set; }

        [JsonPropertyName("lng")]
        public float Longitude { get; set; }
    }
}
