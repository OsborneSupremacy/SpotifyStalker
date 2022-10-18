namespace Spotify.Model;

public record ApiLimits
{
    public SearchLimits Search { get; set; }

    public int UserPlaylist { get; set; }

    public int PlaylistTrack { get; set; }

    public int BatchSize { get; set; }
}

public class ApiLimitsValidator : AbstractValidator<ApiLimits>
{
    public ApiLimitsValidator()
    {
        RuleFor(x => x.Search).NotNull();
        RuleFor(x => x.UserPlaylist).InclusiveBetween(1, 50);
        RuleFor(x => x.PlaylistTrack).InclusiveBetween(1, 100);
    }
}
