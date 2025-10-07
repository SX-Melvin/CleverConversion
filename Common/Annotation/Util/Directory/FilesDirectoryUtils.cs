using CleverConversion.Common.Annotation.Common.Util.Directory;
using CleverConversion.Common.Annotation.Config;

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