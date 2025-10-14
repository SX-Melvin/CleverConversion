using Newtonsoft.Json;
using System.Collections.Generic;

namespace CleverConversion.Common.Common.Entity.Web
{
    /// <summary>
    /// Posted data entity
    /// </summary>
    public class PostedDataEntity
    {
        [JsonProperty]
        public string? Path { get; set; }
        
        [JsonProperty]
        public string? Guid { get; set; }

        [JsonProperty]
        public string? Password { get; set; }

        [JsonProperty]
        public string? Url { get; set; }
        
        [JsonProperty]
        public int Page { get; set; } = 1;

        [JsonProperty]
        public int? Angle { get; set; }

        [JsonProperty]
        public List<int> Pages { get; set; } = [];

        [JsonProperty]
        public bool? Rewrite { get; set; }
    }
}