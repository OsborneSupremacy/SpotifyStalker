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
        RuleFor(x => x.TokenUrl).NotEmpty();
        RuleFor(x => x.TokenUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));

        RuleFor(x => x.SpotifyBaseUrl).NotEmpty();
        RuleFor(x => x.SpotifyBaseUrl)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _));

        RuleFor(x => x.ApiKeys).NotNull();
        RuleFor(x => x.ApiKeys).SetValidator(new ApiKeysValidator());

        RuleFor(x => x.Limits).NotNull();
        RuleFor(x => x.Limits).SetValidator(new ApiLimitsValidator());
    }
}



