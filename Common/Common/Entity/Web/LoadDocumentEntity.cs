using Newtonsoft.Json;

namespace CleverConversion.Common.Common.Entity.Web
{
    public class LoadDocumentEntity
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("pages")]
        public List<PageDescriptionEntity> Pages { get; set; } = [];
    }
}