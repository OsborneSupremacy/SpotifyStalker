using System.Text.Json.Serialization;

namespace Spotify.Object
{
    public class User
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("id")] 
        public string Id { get; set; }
    }
}
