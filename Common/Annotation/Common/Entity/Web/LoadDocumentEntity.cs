using Newtonsoft.Json;
using System.Collections.Generic;

namespace CleverConversion.Common.Annotation.Common.Entity.Web
{
    public class LoadDocumentEntity
    {
        ///Document Guid
        [JsonProperty]
        private string guid;

        ///list of pages        
        [JsonProperty]
        private List<PageDescriptionEntity> pages = new List<PageDescriptionEntity>();

        public void SetGuid(string guid)
        {
            this.guid = guid;
        }

        public string GetGuid()
        {
            return guid;
        }

        public void SetPages(PageDescriptionEntity page)
        {
            pages.Add(page);
        }

        public List<PageDescriptionEntity> GetPages()
        {
            return pages;
        }
    }
}