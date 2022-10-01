namespace Spotify.Model;

public record GenreModel : ISpotifyStandardObject
{
    public string Id => Name;

    public string? Name { get; set; }

    public ConcurrentDictionary<string, ArtistModel>? Artists { get; set; }

    public ConcurrentDictionary<string, Track>? Tracks { get; set; }
}
