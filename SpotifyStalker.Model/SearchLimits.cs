namespace Spotify.Model;

public record SearchLimits
{
    public int Limit { get; set; }

    public int MaximumOffset { get; set; }
}

public class SearchLimitsValidator : AbstractValidator<SearchLimits>
{
    public SearchLimitsValidator()
    {
        RuleFor(x => x.Limit).InclusiveBetween(1, 50);
        RuleFor(x => x.MaximumOffset).InclusiveBetween(1, 1000);
    }
}
