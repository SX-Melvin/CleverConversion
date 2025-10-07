using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using CleverConversion.Common.Annotation.Common.Config;
using CleverConversion.Common.Annotation.Common.Util.Parser;
using System.Text.Json.Serialization;

namespace CleverConversion.Common.Annotation.Config
{
    /// <summary>
    /// AnnotationConfiguration.
    /// </summary>
    public class AnnotationConfiguration : CommonConfiguration
    {
        [JsonProperty]
        public string FilesDirectory { get; set; } = @"C:\Users\user\Documents\Work\Swiftx\CleverConversion\Files";

        [JsonProperty]
        public string DefaultDocument { get; set; } = string.Empty;

        [JsonProperty]
        public int PreloadPageCount { get; set; } = 0;

        [JsonProperty]
        public bool TextAnnotation { get; set; } = true;

        [JsonProperty]
        public bool AreaAnnotation { get; set; } = true;

        [JsonProperty]
        public bool PointAnnotation { get; set; } = true;

        [JsonProperty]
        public bool TextStrikeoutAnnotation { get; set; } = true;

        [JsonProperty]
        public bool PolylineAnnotation { get; set; } = true;

        [JsonProperty]
        public bool TextFieldAnnotation { get; set; } = true;

        [JsonProperty]
        public bool WatermarkAnnotation { get; set; } = true;

        [JsonProperty]
        public bool TextReplacementAnnotation { get; set; } = true;

        [JsonProperty]
        public bool ArrowAnnotation { get; set; } = true;

        [JsonProperty]
        public bool TextRedactionAnnotation { get; set; } = true;

        [JsonProperty]
        public bool ResourcesRedactionAnnotation { get; set; } = false;

        [JsonProperty]
        public bool TextUnderlineAnnotation { get; set; } = true;

        [JsonProperty]
        public bool DistanceAnnotation { get; set; } = true;

        [JsonProperty]
        public bool DownloadOriginal { get; set; } = true;

        [JsonProperty]
        public bool DownloadAnnotated { get; set; } = true;

        [JsonProperty]
        public bool Zoom { get; set; } = true;
    }
}