using CleverConversion.Common.Common.Entity.Web;
using System.Collections.Generic;
using System.IO;
using GroupDocs.Comparison.Result;

namespace CleverConversion.Common.Comparison.Comparison.Model.Request
{
    public class SetChangesRequest
    {
        /// <summary>
        /// Contains list of the documents paths
        /// </summary>
        public List<CompareFileDataRequest> guids { get; set; }

        public int[] changes { get; set; }
    }
}