using CleverConversion.Common.Annotation.Common.Entity.Web;

namespace CleverConversion.Common.Annotation.Entity.Web
{
    /// <summary>
    /// SignaturePostedDataEntity
    /// </summary>
    public class AnnotationPostedDataEntity : PostedDataEntity
    {
        public string? DocumentType { get; set; }
        public AnnotationDataEntity[]? AnnotationsData { get; set; } = [];
        public bool? Print { get; set; } = false;
    }
}