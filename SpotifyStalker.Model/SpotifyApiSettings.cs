namespace Spotify.Model;

public record SpotifyApiSettings
{
    public string TokenUrl { get; set; }

    public string SpotifyBaseUrl { get; set; }

    public ApiKeys ApiKeys { get; set; }

    public ApiLimits Limits { get; set; }
}

public class SpotifyApiSettingsValidator : AbstractValidator<SpotifyApiSettings>
{
    public SpotifyApiSettingsValidator()
    {
        RuleFor(x => x.TokenUrl)
            .NotEmpty()
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));

        RuleFor(x => x.SpotifyBaseUrl).NotEmpty();
        RuleFor(x => x.SpotifyBaseUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));

        RuleFor(x => x.ApiKeys)
            .NotNull()
            .SetValidator(new ApiKeysValidator());

        RuleFor(x => x.Limits)
            .NotNull()
            .SetValidator(new ApiLimitsValidator());
    }
}
