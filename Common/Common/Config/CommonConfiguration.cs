using CleverConversion.Common.Common.Util.Parser;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace CleverConversion.Common.Common.Config
{
    /// <summary>
    /// CommonConfiguration
    /// </summary>
    public class CommonConfiguration
    {
        [JsonProperty]
        public bool PageSelector { get; set; } = true;

        [JsonProperty]
        public bool Download { get; set; } = true;

        [JsonProperty]
        public bool Upload { get; set; } = true;

        [JsonProperty]
        public bool Print { get; set; } = true;

        [JsonProperty]
        public bool Browse { get; set; } = true;

        [JsonProperty]
        public bool Rewrite { get; set; } = true;

        [JsonProperty]
        public bool EnableRightClick { get; set; } = true;
    }
}