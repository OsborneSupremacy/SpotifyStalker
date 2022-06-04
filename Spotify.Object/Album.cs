using System.Collections.Generic;
using System.Text.Json.Serialization;
using Spotify.Interface;

namespace Spotify.Object;

public class Album : ISpotifyStandardObject
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("artists")]
    public IEnumerable<Artist>? Artists { get; set; }
}
