using Newtonsoft.Json;

namespace CleverConversion.Dto.API
{
    public class AddNodeVersionRequest
    {
        public string FilePath {  get; set; }
        
        public long NodeId {  get; set; }
    }
}
