namespace SpotifyStalker.Interface;

public interface IGenreProvider
{
    Task<IEnumerable<string>> GetAsync();
}
