using Newtonsoft.Json;

namespace CleverConversion.Common.Common.Entity.Web
{
    /// <summary>
    /// DocumentDescriptionEntity
    /// </summary>
    public class PageDescriptionEntity
    {
        [JsonProperty("width")]
        public double Width { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("angle")]
        public int Angle { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}