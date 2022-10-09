namespace Spotify.Model;

public record SpotifyApiSettings
{
    [JsonPropertyName("tokenUrl")]
    public string TokenUrl { get; set; }

    [JsonPropertyName("spotifyBaseUrl")]
    [Required(AllowEmptyStrings = false)]
    [Url]
    public string SpotifyBaseUrl { get; set; }

    [JsonPropertyName("apikeys")]
    public ApiKeys ApiKeys { get; set; }

    [JsonPropertyName("limits")]
    [Required]
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
        
        RuleFor(x => x.Limits).NotNull();
        //RuleFor(x => x.Limits).SetValidator(new ApiLimitsValidator());
    }
}



