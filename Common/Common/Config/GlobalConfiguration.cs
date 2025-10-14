using CleverConversion.Common.Annotation.Config;
using CleverConversion.Common.Comparison.Comparison.Config;

namespace CleverConversion.Common.Common.Config
{
    /// <summary>
    /// Global configuration
    /// </summary>
    public class GlobalConfiguration
    {
        public ServerConfiguration Server { get; set; }
        public ApplicationConfiguration Application { get; set; }
        public CommonConfiguration Common { get; set; }
        public AnnotationConfiguration Annotation { get; set; }
        public ComparisonConfiguration Comparison { get; set; }

        /// <summary>
        /// Get all configurations
        /// </summary>
        public GlobalConfiguration()
        {
            Server = new();
            Application = new();
            Common = new();
            Annotation = new();
            Comparison = new();
        }
    }
}