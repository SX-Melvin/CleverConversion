using CleverConversion.Common.Common.Entity.Web;
using GroupDocs.Comparison.Result;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CleverConversion.Common.Comparison.Comparison.Model.Response
{
    public class CompareResultResponse
    {
        /// <summary>
        /// List of changes
        /// </summary>
        [JsonProperty("changes")]
        public ChangeInfo[] Changes { get; set; }

        /// <summary>
        /// List of images of pages with marked changes
        /// </summary>
        [JsonProperty("pages")]
        public List<PageDescriptionEntity> Pages { get; set; }

        /// <summary>
        /// Unique key of results
        /// </summary>
        [JsonProperty("guid")]
        public string Guid { get; set; }

        /// <summary>
        /// Extension of compared files, for saving Comparison results
        /// </summary>
        [JsonProperty("extension")]
        public string Extension { get; set; }

    }
}