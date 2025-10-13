using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleverConversion.Configurations
{
    public class AppConfiguration
    {
        public string BasePath { get; set; }
        public AnnotationConfiguration Annotation { get; set; }
    }

    public class AnnotationConfiguration
    {
        public List<string> AllowedExts { get; set; } = [];
    }
}
