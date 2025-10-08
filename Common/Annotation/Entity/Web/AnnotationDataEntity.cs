
using Newtonsoft.Json;

namespace CleverConversion.Common.Annotation.Entity.Web
{
    public class AnnotationDataEntity
    {
        [JsonProperty]
        public int Id {get; set;}
        
        [JsonProperty]
        public int PageNumber {get; set;}

        [JsonProperty]
        public int? FontColor { get; set; } = 0;

        [JsonProperty]
        public float? FontSize { get; set; } = 0;

        [JsonProperty]
        public float Left {get; set;}

        [JsonProperty]
        public float Top {get; set;}

        [JsonProperty]
        public float Width { get; set;}

        [JsonProperty]
        public float Height {get; set;}
        
        [JsonProperty]
        public string? SvgPath {get; set;}
        
        [JsonProperty]
        public string? Type {get; set;}
        
        [JsonProperty]
        public string? DocumentType {get; set;}
        
        [JsonProperty]
        public string? Text {get; set;}
        
        [JsonProperty]
        public string? Font {get; set;}

        [JsonProperty]
        public bool? Imported { get; set; }

        [JsonProperty]
        public CommentsEntity[]? Comments { get; set; } = [];
    }
}