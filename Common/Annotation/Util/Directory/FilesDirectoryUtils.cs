using CleverConversion.Common.Annotation.Config;
using CleverConversion.Common.Common.Util.Directory;

namespace CleverConversion.Common.Annotation.Util.Directory
{
    public class FilesDirectoryUtils : IDirectoryUtils
    {

        private readonly AnnotationConfiguration AnnotationConfiguration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="signatureConfiguration">SignatureConfiguration</param>
        public FilesDirectoryUtils(AnnotationConfiguration annotationConfiguration)
        {
            AnnotationConfiguration = annotationConfiguration;
        }

        /// <summary>
        /// Get path
        /// </summary>
        /// <returns>string</returns>
        public string GetPath()
        {
            return AnnotationConfiguration.FilesDirectory;
        }
    }
}