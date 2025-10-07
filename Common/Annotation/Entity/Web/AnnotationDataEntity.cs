
namespace CleverConversion.Common.Annotation.Entity.Web
{
    public class AnnotationDataEntity
    {
        public int? Id {get; set;}
        public int? PageNumber {get; set;}
        public int? FontColor { get; set; }
        public float? FontSize {get; set;}
        public float? Left {get; set;}
        public float? Top {get; set;}
        public float? Width { get; set;}
        public float? Height {get; set;}
        public string? SvgPath {get; set;}
        public string? Type {get; set;}
        public string? DocumentType {get; set;}
        public string? Text {get; set;}
        public string? Font {get; set;}        
        public bool? Imported { get; set; }
        public CommentsEntity[]? Comments { get; set; } = [];
    }
}