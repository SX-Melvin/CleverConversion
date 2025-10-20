using CleverConversion.Common.Common.Entity.Web;
using Newtonsoft.Json;

namespace CleverConversion.Common.Annotation.Entity.Web
{
    public class AnnotatedDocumentEntity : PageDescriptionEntity
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }
        
        [JsonProperty("pages")]
        public List<PageDataDescriptionEntity> Pages { get; set; } = [];
        
        [JsonProperty("supportedAnnotations")]
        public string[] SupportedAnnotations { get; set; }
    }
}