using Newtonsoft.Json;

namespace CleverConversion.Common.Common.Entity.Web
{
    public class LoadDocumentEntity
    {
        [JsonProperty]
        public string Guid { get; set; }

        [JsonProperty]
        public List<PageDescriptionEntity> Pages { get; set; } = [];
    }
}