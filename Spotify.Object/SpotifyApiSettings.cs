using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class SpotifyApiSettings
    {
        [JsonPropertyName("tokenUrl")]
        [Required(AllowEmptyStrings = false)]
        [Url]
        public string TokenUrl { get; set; }

        [JsonPropertyName("spotifyBaseUrl")]
        [Required(AllowEmptyStrings = false)]
        [Url]
        public string SpotifyBaseUrl { get; set; }

        [JsonPropertyName("apikeys")]
        public ApiKeys ApiKeys { get; set; }
    }

    public class ApiKeys
    {
        [JsonPropertyName("clientid")]
        public string ClientId { get; set; }

        [JsonPropertyName("clientsecret")]
        public string ClientSecret { get; set; }
    }
}
