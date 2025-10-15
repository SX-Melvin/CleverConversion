
using Newtonsoft.Json;

namespace CleverConversion.Common.Comparison.Comparison.Model.Request
{
    public class CompareFileDataRequest
    {
        [JsonProperty]
        public string Guid { get; set; }

        [JsonProperty]
        public string? Password { get; set; } = null;
    }
}