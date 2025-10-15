using Newtonsoft.Json;

namespace CleverConversion.Dto.OTCS
{
    public class AddNodeVersionResponse : CommonResponse
    {
        [JsonProperty]
        public AddNodeVersionResult Results { get; set; }
    }

    public class AddNodeVersionResult
    {
        [JsonProperty]
        public Node Data { get; set; }
    }
}
