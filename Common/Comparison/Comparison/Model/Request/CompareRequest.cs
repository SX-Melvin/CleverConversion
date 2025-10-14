using CleverConversion.Common.Common.Entity.Web;
using System.Collections.Generic;
using System.IO;

namespace CleverConversion.Common.Comparison.Comparison.Model.Request
{
    public class CompareRequest
    {
        /// <summary>
        /// Contains list of the documents paths
        /// </summary>
        public List<CompareFileDataRequest> guids { get; set; }
    }
}