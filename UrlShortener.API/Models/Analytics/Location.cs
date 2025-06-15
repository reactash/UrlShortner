using System.Text.Json.Serialization;

namespace UrlShortener.API.Models.Analytics
{
    public class Location
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("isp")]
        public string Isp { get; set; }

        [JsonPropertyName("regionName")]
        public string RegionName { get; set; }

        [JsonPropertyName("zip")]
        public string Zip { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }
}