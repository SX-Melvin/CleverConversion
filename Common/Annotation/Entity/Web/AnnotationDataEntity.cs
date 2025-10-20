
using Newtonsoft.Json;

namespace CleverConversion.Common.Annotation.Entity.Web
{
    public class AnnotationDataEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("fontColor")]
        public int? FontColor { get; set; }

        [JsonProperty("fontSize")]
        public float? FontSize { get; set; }

        [JsonProperty("left")]
        public float Left { get; set; }

        [JsonProperty("top")]
        public float Top { get; set; }

        [JsonProperty("width")]
        public float Width { get; set; }

        [JsonProperty("height")]
        public float Height { get; set; }

        [JsonProperty("svgPath")]
        public string? SvgPath { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("documentType")]
        public string? DocumentType { get; set; }

        [JsonProperty("text")]
        public string? Text { get; set; }

        [JsonProperty("font")]
        public string? Font { get; set; }

        [JsonProperty("imported")]
        public bool? Imported { get; set; }

        [JsonProperty("comments")]
        public CommentsEntity[]? Comments { get; set; } = [];

    }
}