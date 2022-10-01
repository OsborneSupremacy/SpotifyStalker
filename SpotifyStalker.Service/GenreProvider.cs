namespace SpotifyStalker.Service;

public class GenreProvider : IGenreProvider
{
    private readonly IFileContentProvider _fileContentProvider;

    private readonly IMemoryCacheService _memoryCacheService;

    public GenreProvider(IFileContentProvider fileContentProvider, IMemoryCacheService memoryCacheService)
    {
        _fileContentProvider = fileContentProvider ?? throw new ArgumentNullException(nameof(fileContentProvider));
        _memoryCacheService = memoryCacheService ?? throw new ArgumentNullException(nameof(memoryCacheService));
    }

    public async Task<IEnumerable<string>> GetAsync() =>
        await _memoryCacheService.GetOrCreateAsync("_genres", async () =>
        {
            var artists = _fileContentProvider.GetEnumerable(@"Files", "genres.txt");
            return await Task.FromResult(artists);
        });
}
