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
        [JsonProperty]
        public string FilesDirectory { get; set; } = @".\Files";

        [JsonProperty]
        public string ResultDirectory { get; set; } = @".\Files\Compared";

        [JsonProperty]
        public int PreloadResultPageCount { get; set; }
    }
}