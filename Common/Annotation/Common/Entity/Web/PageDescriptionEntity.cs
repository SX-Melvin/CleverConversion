using Newtonsoft.Json;

namespace CleverConversion.Common.Annotation.Common.Entity.Web
{
    /// <summary>
    /// DocumentDescriptionEntity
    /// </summary>
    public class PageDescriptionEntity
    {
        [JsonProperty]
        public double Width { get; set; }

        [JsonProperty]
        public int Number { get; set; }

        [JsonProperty]
        public double Height { get; set; }

        [JsonProperty]
        public int Angle { get; set; }

        [JsonProperty]
        public string Data { get; set; }

    }
}