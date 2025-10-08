
using Newtonsoft.Json;

namespace CleverConversion.Common.Annotation.Entity.Web
{
    public class CommentsEntity
    {
        [JsonProperty]
        public string Time { get; set; }
        
        [JsonProperty]
        public string Text { get; set; }
        
        [JsonProperty]
        public string UserName { get; set; }
    }
}