using Newtonsoft.Json;

namespace NugetPackage.WebApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Args
    {
    }

    public class Headers
    {
        public string Accept { get; set; }

        [JsonProperty("Accept-Encoding")]
        public string AcceptEncoding { get; set; }

        [JsonProperty("Cache-Control")]
        public string CacheControl { get; set; }
        public string Host { get; set; }

        [JsonProperty("Postman-Token")]
        public string PostmanToken { get; set; }

        [JsonProperty("User-Agent")]
        public string UserAgent { get; set; }

        [JsonProperty("X-Amzn-Trace-Id")]
        public string XAmznTraceId { get; set; }
    }

    public class Root
    {
        public Args args { get; set; }
        public Headers headers { get; set; }
        public string origin { get; set; }
        public string url { get; set; }
    }



}
