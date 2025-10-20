using Newtonsoft.Json;
using System.Collections.Generic;

namespace CleverConversion.Common.Common.Entity.Web
{
    /// <summary>
    /// Posted data entity
    /// </summary>
    public class PostedDataEntity
    {
        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        [JsonProperty("angle")]
        public int? Angle { get; set; }

        [JsonProperty("pages")]
        public List<int> Pages { get; set; } = [];

        [JsonProperty("rewrite")]
        public bool? Rewrite { get; set; }
    }
}