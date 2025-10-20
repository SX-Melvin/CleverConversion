using Newtonsoft.Json;

namespace CleverConversion.Common.Common.Entity.Web
{
    /// <summary>
    /// File description entity
    /// </summary>
    public class FileDescriptionEntity
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("docType")]
        public string DocType { get; set; }

        [JsonProperty("isDirectory")]
        public bool IsDirectory { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }
}