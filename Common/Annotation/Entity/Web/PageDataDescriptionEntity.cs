using CleverConversion.Common.Annotation.Common.Entity.Web;
using Newtonsoft.Json;

namespace CleverConversion.Common.Annotation.Entity.Web
{
    public class PageDataDescriptionEntity : PageDescriptionEntity
    {
        /// List of annotation data  
        [JsonProperty]
        public AnnotationDataEntity[] Annotations { get; set; } = [];
    }
}