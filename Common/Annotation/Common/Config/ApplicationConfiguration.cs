using CleverConversion.Common.Annotation.Common.Util.Parser;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CleverConversion.Common.Annotation.Common.Config
{
    /// <summary>
    /// Application configuration
    /// </summary>
    public class ApplicationConfiguration
    {
        /// <summary>
        /// Get license path from the application configuration section of the web.config
        /// </summary>
        public ApplicationConfiguration()
        {
            try
            {
                YamlParser parser = new();
                dynamic configuration = parser.GetConfiguration("application");
                ConfigurationValuesGetter valuesGetter = new ConfigurationValuesGetter(configuration);
                string license = valuesGetter.GetStringPropertyValue("licensePath");
                if (string.IsNullOrEmpty(license))
                {
                    string[] files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LicensePath), "*.lic");
                    LicensePath = Path.Combine(LicensePath, files[0]);
                }
                else
                {
                    if (!IsFullPath(license))
                    {
                        license = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, license);
                        if (!Directory.Exists(Path.GetDirectoryName(license)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(license));
                        }
                    }
                    LicensePath = license;
                    if (!File.Exists(LicensePath))
                    {
                        Debug.WriteLine("License file path is incorrect, launched in trial mode");
                        LicensePath = "";
                    }
                }
            }
            catch (Exception)
            {
                LicensePath = "";
            }
        }

        public string LicensePath { get; set; } = "Licenses";

        private static bool IsFullPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path)
                && path.IndexOfAny(Path.GetInvalidPathChars().ToArray()) == -1
                && Path.IsPathRooted(path)
                && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }
    }
}