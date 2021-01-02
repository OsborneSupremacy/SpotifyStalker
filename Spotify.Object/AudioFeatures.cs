using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class AudioFeatures
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("danceability")]
        public double? Danceability { get; set; }

        [JsonPropertyName("energy")]
        public double? Energy { get; set; }

        [JsonPropertyName("key")]
        public double? Key { get; set; }

        [JsonPropertyName("loudness")]
        public double? Loudness { get; set; }

        [JsonPropertyName("mode")]
        public double? Mode { get; set; }

        [JsonPropertyName("speechiness")]
        public double? Speechiness { get; set; }

        [JsonPropertyName("acousticness")]
        public double? Acousticness { get; set; }

        [JsonPropertyName("instrumentalness")]
        public double? Instrumentalness { get; set; }

        [JsonPropertyName("liveness")]
        public double? Liveness { get; set; }

        [JsonPropertyName("valence")]
        public double? Valence { get; set; }

        [JsonPropertyName("tempo")]
        public double? Tempo { get; set; }

        [JsonPropertyName("duration_ms")]
        public double? DurationMs { get; set; }

        [JsonPropertyName("time_signature")]
        public double? TimeSignature { get; set; }
    }
}
