using System.Collections.Generic;

namespace CleverConversion.Common.Annotation.Common.Entity.Web
{
    /// <summary>
    /// Posted data entity
    /// </summary>
    public class PostedDataEntity
    {
        public string? path { get; set; }
        public string? guid { get; set; }
        public string? password { get; set; }
        public string? url { get; set; }
        public int page { get; set; } = 1;
        public int? angle { get; set; }
        public List<int> pages { get; set; } = [];
        public bool? rewrite { get; set; }
    }
}