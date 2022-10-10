namespace Spotify.Model;

public record ApiKeys
{
    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
}

public class ApiKeysValidator : AbstractValidator<ApiKeys>
{
    public ApiKeysValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.ClientSecret).NotEmpty();
    }
}
