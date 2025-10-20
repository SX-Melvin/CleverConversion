using Newtonsoft.Json;

namespace CleverConversion.Common.Common.Entity.Web
{
    /// <summary>
    /// Uploaded document entity
    /// </summary>
    public class UploadedDocumentEntity
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }
    }
}