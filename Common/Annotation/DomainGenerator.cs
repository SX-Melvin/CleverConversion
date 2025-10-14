using CleverConversion.Common.Common.Config;
using System;
using System.IO;
using System.Reflection;

namespace CleverConversion.Common.Annotation
{
    /// <summary>
    /// DomainGenerator
    /// </summary>
    public class DomainGenerator
    {
        private readonly GlobalConfiguration globalConfiguration;
        private readonly Type CurrentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public DomainGenerator(string assemblyName, string className)
        {
            globalConfiguration = new Common.Config.GlobalConfiguration();
            // Get assembly path
            string assemblyPath = this.GetAssemblyPath(assemblyName);
            // Initiate GroupDocs license class
            CurrentType = this.CreateDomain(assemblyName + "Domain", assemblyPath, className);
        }

        /// <summary>
        /// Get assembly full path by its name
        /// </summary>
        /// <param name="assemblyName">string</param>
        /// <returns></returns>
        private string GetAssemblyPath(string assemblyName)
        {
            string path = "";
            // Get path of the executable
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string uriPath = Uri.UnescapeDataString(uri.Path);
            // Get path of the assembly
            path = Path.Combine(Path.GetDirectoryName(uriPath), assemblyName);
            return path;
        }

        /// <summary>
        /// Create AppDomain for the assembly
        /// </summary>
        /// <param name="domainName">string</param>
        /// <param name="assemblyPath">string</param>
        /// <param name="className">string</param>
        /// <returns></returns>
        private Type CreateDomain(string domainName, string assemblyPath, string className)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly.GetType(className);
        }      

        /// <summary>
        /// Set GroupDocs.Annotation license
        /// </summary>       
        public void SetAnnotationLicense()
        {
            // Initiate license class
            var obj = (GroupDocs.Annotation.License)Activator.CreateInstance(CurrentType);
            // Set license
            SetLicense(obj);
        }        

        private void SetLicense(dynamic obj)
        {
            if (!String.IsNullOrEmpty(globalConfiguration.Application.LicensePath))
            {
                obj.SetLicense(globalConfiguration.Application.LicensePath);
            }
        }
    }
}