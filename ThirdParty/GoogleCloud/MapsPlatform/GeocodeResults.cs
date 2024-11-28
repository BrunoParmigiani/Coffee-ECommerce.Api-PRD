using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.ThirdParty.GoogleCloud.MapsPlatform
{
    public sealed class GeocodeResults
    {
        [JsonPropertyName("address_components")]
        public AddressComponents[] AddressComponents { get; set; }

        [JsonPropertyName("formatted_address")]
        public string FormatedAddress { get; set; }

        [JsonPropertyName("geometry")]
        public Dictionary<string, object> Geometry { get; set; }

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }

        [JsonPropertyName("postcode_localities")]
        public string[] PostcodeLocalities { get; set; }

        [JsonPropertyName("types")]
        public string[] Types { get; set; }
    }
}
