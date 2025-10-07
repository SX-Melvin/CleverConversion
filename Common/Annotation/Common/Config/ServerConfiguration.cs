using CleverConversion.Common.Annotation.Common.Util.Parser;
using System.Collections.Specialized;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using ConfigurationSection = System.Configuration.ConfigurationSection;

namespace CleverConversion.Common.Annotation.Common.Config
{
    /// <summary>
    /// Server configuration
    /// </summary>
    public class ServerConfiguration : ConfigurationSection
    {
        private readonly NameValueCollection serverConfiguration = (NameValueCollection)ConfigurationManager.GetSection("serverConfiguration");

        public int HttpPort { get; set; } = 8080;
        public string HostAddress { get; set; } = "localhost";

        /// <summary>
        /// Get server configuration section of the web.config
        /// </summary>
        public ServerConfiguration() {
            YamlParser parser = new YamlParser();
            dynamic configuration = parser.GetConfiguration("server");
            ConfigurationValuesGetter valuesGetter = new(configuration);
            int defaultPort = 8080;
            HttpPort = valuesGetter.GetIntegerPropertyValue("connector", defaultPort, "port");
            HostAddress = valuesGetter.GetStringPropertyValue("hostAddress", HostAddress);
        }
    }
}