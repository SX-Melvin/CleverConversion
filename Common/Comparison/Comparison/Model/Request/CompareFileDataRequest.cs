
using Newtonsoft.Json;

namespace CleverConversion.Common.Comparison.Comparison.Model.Request
{
    public class CompareFileDataRequest
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; } = null;
    }
}