using Newtonsoft.Json;

namespace RunGroupWebApp.Helpers
{
    public class IPInfo
    {
        //Json property - used to match the property with the keys of the json result.
        //Since, we prefer to use different name or case in this class's properties.
        [JsonProperty("ip")]
        public string Ip { get; set; }
        [JsonProperty("hostname")]
        public string Hostname { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Location { get; set; }
        [JsonProperty("org")]
        public string Org { get; set; }
        [JsonProperty("postal")]
        public string Postal { get; set; }
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }
}
