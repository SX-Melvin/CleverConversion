using Newtonsoft.Json;

namespace CleverConversion.Dto.OTCS
{
    public class Node
    {
        [JsonProperty]
        public NodeProperty Properties { get; set; }
    }

    public class NodeProperty
    {
        [JsonProperty]
        public bool Container { get; set; }
        
        [JsonProperty]
        public long Id { get; set; }
        
        [JsonProperty]
        public string Name { get; set; }
    }
}
