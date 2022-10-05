namespace SpotifyStalker.ConsoleUi;

[ServiceLifetime(ServiceLifetime.Singleton)]
public class ArtistQueryService
{
    private readonly ILogger<ArtistQueryService> _logger;

    private readonly IApiQueryService _apiQueryService;

    private readonly SpotifyApiSettings _spotifyApiSettings;

    private readonly SearchTermBuilderService _searchTermBuilderService;

    private readonly SpotifyStalkerDbContext _dbContext;

    public ArtistQueryService(
        ILogger<ArtistQueryService> logger,
        IOptions<SpotifyApiSettings> settings,
        IApiQueryService apiQueryService,
        SearchTermBuilderService searchTermBuilderService,
        SpotifyStalkerDbContext dbContext
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _spotifyApiSettings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
        _searchTermBuilderService = searchTermBuilderService ?? throw new ArgumentNullException(nameof(searchTermBuilderService));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task ExecuteAsync()
    {
        _logger.LogDebug("Querying Artists");

        var searchTerms = _searchTermBuilderService.GenerateSearchTerms();

        foreach (var searchTerm in searchTerms)
            await QueryUsingSearchTermAsync(searchTerm);
    }

    public async Task QueryUsingSearchTermAsync(string searchTerm)
    {
        var queryLog = _dbContext.ArtistQueryLogs.Where(x => x.SearchTerm == searchTerm).FirstOrDefault();

        if (queryLog != null && queryLog.CompletedDate.HasValue)
        {
            _logger.LogDebug("{searchTerm} already queried. Skipping", searchTerm);
            return;
        }

        _logger.LogDebug("Querying Artists with {searchTerm}", searchTerm);

        if (queryLog is null)
        {
            queryLog = new ArtistQueryLog() { SearchTerm = searchTerm, QueriedDate = DateTime.Now };
            await _dbContext.AddAsync(queryLog);
            await _dbContext.SaveChangesAsync();
            queryLog = _dbContext.ArtistQueryLogs.Where(x => x.SearchTerm == searchTerm).First();
        }

        queryLog.QueriedDate = DateTime.Now;

        await QueryArtistsUsingSearchTermAsync(searchTerm, resultCountUpdater, saveToDatabase);

        queryLog.CompletedDate = DateTime.Now;
        _dbContext.Update(queryLog);
        await _dbContext.SaveChangesAsync();

        void resultCountUpdater(int resultCount)
        {
            queryLog.ResultCount = resultCount;
            _dbContext.Update(queryLog);
        }

        async Task saveToDatabase(Spotify.Object.Artist artist)
        {
            // 1. Remove existing record from database
            _dbContext.Artists.RemoveRange(_dbContext.Artists.Where(x => x.ArtistId == artist.Id));
            // 2. add new record
            _dbContext.Add(new Artist()
            {
                ArtistId = artist.Id,
                ArtistName = artist.Name,
                Genres = string.Join("|", artist.Genres),
                Popularity = artist.Popularity
            });
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task QueryArtistsUsingSearchTermAsync(string searchTerm,
        Action<int> resultCountUpdater,
        Func<Spotify.Object.Artist, Task> saveToDatabase
        )
    {
        var itemsQueried = 0;
        var totalItems = 1;
        bool firstIteration = true;

        var artists = new Dictionary<string, Spotify.Object.Artist>();

        while (itemsQueried < totalItems)
        {
            var result = await _apiQueryService.QueryAsync<ArtistSearchResultModel>(searchTerm, _spotifyApiSettings.Limits.Search.Limit, itemsQueried);
            itemsQueried += _spotifyApiSettings.Limits.Search.Limit;

            await result.Match
            (
                async success =>
                {
                    // only need to read this info once, so do it on first iteration
                    if (firstIteration)
                    {
                        GetTotalItems(success, resultCountUpdater);
                        firstIteration = false;
                    }

                    foreach (var item in success.Artists.Items)
                    {
                        // skip artists with 0 popularity or unknown genres. There are a ton of them in Spotify, and for our
                        // purposes we don't want them.
                        if (item.Popularity == 0 || !(item.Genres?.Any() ?? false)) continue;

                        // if artist is already in dictionary, don't need to do anything else
                        if (!artists.TryAdd(item.Id, item)) continue;

                        await saveToDatabase.Invoke(item);
                    }

                    return true;
                },
                exception =>
                {
                    return Task.FromResult(true);
                }
            );
        } // loop through items
    }

    private void GetTotalItems(
        ArtistSearchResultModel resultModel,
        Action<int> resultCountUpdater
        )
    {
        var totalItems = resultModel.Artists.Total;
        if (totalItems > _spotifyApiSettings.Limits.Search.MaximumOffset)
            totalItems = _spotifyApiSettings.Limits.Search.MaximumOffset;

        resultCountUpdater.Invoke(totalItems);

        _logger.LogDebug("Total Items: {totalItems}", totalItems);
    }
}
