using System.Collections.Generic;

namespace CleverConversion.Common.Viewer.Entities
{
    public class DocumentInfo
    {
        public string FileType { get; set; }

        public bool PrintAllowed { get; set; }

        public IEnumerable<PageInfo> Pages { get; set; }
    }
}