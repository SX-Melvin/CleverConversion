using Newtonsoft.Json;

namespace CleverConversion.Common.Common.Entity.Web
{
    /// <summary>
    /// File description entity
    /// </summary>
    public class FileDescriptionEntity
    {
        [JsonProperty]
        public string Guid{ get; set; }

        [JsonProperty]
        public string Name{ get; set; }
        
        [JsonProperty]
        public string DocType{ get; set; }
        
        [JsonProperty]
        public bool IsDirectory{ get; set; }
        
        [JsonProperty]
        public long Size{ get; set; }
    }
}