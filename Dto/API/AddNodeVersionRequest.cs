using Newtonsoft.Json;

namespace CleverConversion.Dto.API
{
    public class AddNodeVersionRequest
    {
        [JsonProperty]
        public string FilePath {  get; set; }
        
        [JsonProperty]
        public long NodeId {  get; set; }
    }
}
