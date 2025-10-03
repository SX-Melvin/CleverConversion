using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleverConversion.Configurations
{
    public class OTCSConfiguration
    {
        public string Username { get; set; }
        public string Secret { get; set; }
        public string Url { get; set; }
    }
}
