using CleverConversion.Common.Annotation.Common.Entity.Web;
using Newtonsoft.Json;

namespace CleverConversion.Common.Annotation.Entity.Web
{
    public class AnnotatedDocumentEntity : PageDescriptionEntity
    {
        [JsonProperty]
        public string Guid { get; set; }
        
        [JsonProperty]
        public List<PageDataDescriptionEntity> Pages { get; set; } = [];
        
        [JsonProperty]
        public string[] SupportedAnnotations { get; set; }
    }
}