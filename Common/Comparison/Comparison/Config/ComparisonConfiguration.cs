using System;
using System.IO;
using System.Linq;
using CleverConversion.Common.Common.Config;
using CleverConversion.Common.Common.Util.Parser;
using Newtonsoft.Json;

namespace CleverConversion.Common.Comparison.Comparison.Config
{
    /// <summary>
    /// CommonConfiguration
    /// </summary>
    public class ComparisonConfiguration : CommonConfiguration
    {
        [JsonProperty("filesDirectory")]
        public string FilesDirectory { get; set; } = @"C:\Users\user\Documents\Work\Swiftx\CleverConversion\Files";

        [JsonProperty("resultDirectory")]
        public string ResultDirectory { get; set; } = @"C:\Users\user\Documents\Work\Swiftx\CleverConversion\Compared";

        [JsonProperty("preloadResultPageCount")]
        public int PreloadResultPageCount { get; set; }
    }
}