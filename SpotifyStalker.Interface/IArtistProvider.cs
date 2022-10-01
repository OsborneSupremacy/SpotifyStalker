namespace SpotifyStalker.Interface;

public interface IArtistProvider
{
    Task<IEnumerable<string>> GetAsync();
}
