using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class Metric
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("imagefile")]
        public string ImageFile { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("min")]
        public double? Min { get; set; }

        [JsonPropertyName("max")]
        public double? Max { get; set; }

        public double? NominalMin { get; set; }

        public double? NominalMax { get; set; }

        [JsonPropertyName("value")]
        public double? Value { get; set; }

        public double? MarkerPercentage { get; set; }
    }
}
