
using Newtonsoft.Json;

namespace CleverConversion.Common.Annotation.Entity.Web
{
    public class CommentsEntity
    {
        [JsonProperty("time")]
        public string Time { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}