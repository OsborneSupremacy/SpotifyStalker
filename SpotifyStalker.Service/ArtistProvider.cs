namespace SpotifyStalker.Service;

public class ArtistProvider : IArtistProvider
{
    private readonly IFileContentProvider _fileContentProvider;

    private readonly IMemoryCacheService _memoryCacheService;

    public ArtistProvider(IFileContentProvider fileContentProvider, IMemoryCacheService memoryCacheService)
    {
        _fileContentProvider = fileContentProvider ?? throw new ArgumentNullException(nameof(fileContentProvider));
        _memoryCacheService = memoryCacheService ?? throw new ArgumentNullException(nameof(memoryCacheService));
    }

    public async Task<IEnumerable<string>> GetAsync() =>
        await _memoryCacheService.GetOrCreateAsync("_artists", async () =>
        {
            var artists = _fileContentProvider.GetEnumerable("Files", "artists.txt");
            return await Task.FromResult(artists);
        });
}
