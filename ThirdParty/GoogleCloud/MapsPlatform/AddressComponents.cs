using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.API.ThirdParty.GoogleCloud.MapsPlatform
{
    public sealed class AddressComponents
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("types")]
        public string[] Types { get; set; }
    }
}
