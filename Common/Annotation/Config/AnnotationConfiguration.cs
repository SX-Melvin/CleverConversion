using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using CleverConversion.Common.Common.Util.Parser;
using System.Text.Json.Serialization;
using CleverConversion.Common.Common.Config;

namespace CleverConversion.Common.Annotation.Config
{
    /// <summary>
    /// AnnotationConfiguration.
    /// </summary>
    public class AnnotationConfiguration : CommonConfiguration
    {
        [JsonProperty("filesDirectory")]
        public string FilesDirectory { get; set; } = @"C:\Users\user\Documents\Work\Swiftx\CleverConversion\Files";

        [JsonProperty("defaultDocument")]
        public string DefaultDocument { get; set; } = string.Empty;

        [JsonProperty("preloadPageCount")]
        public int PreloadPageCount { get; set; } = 0;

        [JsonProperty("textAnnotation")]
        public bool TextAnnotation { get; set; } = true;

        [JsonProperty("areaAnnotation")]
        public bool AreaAnnotation { get; set; } = true;

        [JsonProperty("pointAnnotation")]
        public bool PointAnnotation { get; set; } = true;

        [JsonProperty("textStrikeoutAnnotation")]
        public bool TextStrikeoutAnnotation { get; set; } = true;

        [JsonProperty("polylineAnnotation")]
        public bool PolylineAnnotation { get; set; } = true;

        [JsonProperty("textFieldAnnotation")]
        public bool TextFieldAnnotation { get; set; } = true;

        [JsonProperty("watermarkAnnotation")]
        public bool WatermarkAnnotation { get; set; } = true;

        [JsonProperty("textReplacementAnnotation")]
        public bool TextReplacementAnnotation { get; set; } = true;

        [JsonProperty("arrowAnnotation")]
        public bool ArrowAnnotation { get; set; } = true;

        [JsonProperty("textRedactionAnnotation")]
        public bool TextRedactionAnnotation { get; set; } = true;

        [JsonProperty("resourcesRedactionAnnotation")]
        public bool ResourcesRedactionAnnotation { get; set; } = false;

        [JsonProperty("textUnderlineAnnotation")]
        public bool TextUnderlineAnnotation { get; set; } = true;

        [JsonProperty("distanceAnnotation")]
        public bool DistanceAnnotation { get; set; } = true;

        [JsonProperty("downloadOriginal")]
        public bool DownloadOriginal { get; set; } = true;

        [JsonProperty("downloadAnnotated")]
        public bool DownloadAnnotated { get; set; } = true;

        [JsonProperty("zoom")]
        public bool Zoom { get; set; } = true;
    }
}