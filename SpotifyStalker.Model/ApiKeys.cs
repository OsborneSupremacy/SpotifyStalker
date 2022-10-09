namespace Spotify.Model;

public record ApiKeys
{
    [JsonPropertyName("clientid")]
    public string ClientId { get; set; }

    [JsonPropertyName("clientsecret")]
    public string ClientSecret { get; set; }
}

public class ApiKeysValidator : AbstractValidator<ApiKeys>
{
    public ApiKeysValidator()
    {
        RuleFor(x => x.ClientId).NotNull();
        RuleFor(x => x.ClientSecret).NotNull();
    }
}
