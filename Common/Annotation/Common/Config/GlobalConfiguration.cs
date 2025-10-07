using CleverConversion.Common.Annotation.Config;

namespace CleverConversion.Common.Annotation.Common.Config
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

        /// <summary>
        /// Get all configurations
        /// </summary>
        public GlobalConfiguration()
        {
            Server = new ServerConfiguration();
            Application = new ApplicationConfiguration();
            Common = new CommonConfiguration();
            Annotation = new AnnotationConfiguration();
        }
    }
}