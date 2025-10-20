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
        [JsonProperty("pageSelector")]
        public bool PageSelector { get; set; } = true;

        [JsonProperty("download")]
        public bool Download { get; set; } = true;

        [JsonProperty("upload")]
        public bool Upload { get; set; } = true;

        [JsonProperty("print")]
        public bool Print { get; set; } = true;

        [JsonProperty("browse")]
        public bool Browse { get; set; } = true;

        [JsonProperty("rewrite")]
        public bool Rewrite { get; set; } = true;

        [JsonProperty("enableRightClick")]
        public bool EnableRightClick { get; set; } = true;

    }
}