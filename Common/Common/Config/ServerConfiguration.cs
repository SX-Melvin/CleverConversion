using CleverConversion.Common.Common.Util.Parser;
using System.Collections.Specialized;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using ConfigurationSection = System.Configuration.ConfigurationSection;

namespace CleverConversion.Common.Common.Config
{
    /// <summary>
    /// Server configuration
    /// </summary>
    public class ServerConfiguration : ConfigurationSection
    {
        public int HttpPort { get; set; } = 8080;
        public string HostAddress { get; set; } = "localhost";
    }
}