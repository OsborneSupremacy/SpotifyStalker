using Bogus;

namespace SpotifyStalker.Service;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IRandomProvider))]
public class RandomProvider : IRandomProvider
{
    private readonly IArtistProvider _artistProvider;

    private readonly IGenreProvider _genreProvider;

    public RandomProvider(IArtistProvider artistProvider, IGenreProvider genreProvider)
    {
        _artistProvider = artistProvider ?? throw new ArgumentNullException(nameof(artistProvider));
        _genreProvider = genreProvider ?? throw new ArgumentNullException(nameof(genreProvider));
    }

    public string GetPersonName() => new Faker().Person.FirstName;

    public string GetWord() => new Randomizer().Word().ToString();

    public T PickRandom<T>(IEnumerable<T> items) => new Faker().PickRandom(items);

    public string GetLocation() => new Faker().Address.Country().ToString();

    public string GetArtist()
    {
        var artists = _artistProvider.GetAsync().GetAwaiter().GetResult().ToList();
        return PickRandom(artists);
    }

    public string GetGenre()
    {
        var genres = _genreProvider.GetAsync().GetAwaiter().GetResult().ToList();
        return PickRandom(genres);
    }
}
