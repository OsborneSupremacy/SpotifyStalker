namespace Spotify.Model;

public record ApiLimits
{
    [JsonPropertyName("search")]
    public SearchLimits Search { get; set; }

    [JsonPropertyName("userplaylist")]
    public int UserPlaylist { get; set; }

    [JsonPropertyName("playlisttrack")]
    public int PlaylistTrack { get; set; }

    [JsonPropertyName("batchsize")]
    [Required]
    [Range(1, 50)]
    public int BatchSize { get; set; }
}

public class ApiLimitsValidator : AbstractValidator<ApiLimits>
{
    public ApiLimitsValidator()
    {
        RuleFor(x => x.Search).NotNull();
        RuleFor(x => x.UserPlaylist).InclusiveBetween(1, 50);
        RuleFor(x => x.PlaylistTrack).InclusiveBetween(1, 100);
        RuleFor(x => x.PlaylistTrack).InclusiveBetween(1, 50);
    }
}
