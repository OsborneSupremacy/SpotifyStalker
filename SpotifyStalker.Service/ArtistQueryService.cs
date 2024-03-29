﻿
namespace SpotifyStalker.Service;

[ServiceLifetime(ServiceLifetime.Scoped)]
[RegistrationTarget(typeof(IArtistQueryService))]
public class ArtistQueryService : IArtistQueryService
{
    private readonly IApiBatchQueryService<ArtistModelCollection> _artistBatchQueryService;

    private readonly IStalkModelTransformer _stalkModelTransformer;

    public ArtistQueryService(
        IApiBatchQueryService<ArtistModelCollection> artistBatchQueryService,
        IStalkModelTransformer stalkModelTransformer
        )
    {
        _artistBatchQueryService = artistBatchQueryService ?? throw new ArgumentNullException(nameof(artistBatchQueryService));
        _stalkModelTransformer = stalkModelTransformer ?? throw new ArgumentNullException(nameof(stalkModelTransformer));
    }

    public async Task QueryAsync(
        StalkModel viewModel,
        Action<string> statusUpdateCallback,
        Action<int> incrementCountCallback
        )
    {
        statusUpdateCallback("Looking up artists");

        // add all artists to batch query service
        foreach (var artist in viewModel.Artists.Items)
            _artistBatchQueryService.AddToQueue(artist.Key);

        while (!_artistBatchQueryService.QueueIsEmpty())
        {
            var (artistResult, artistItemsQueried) = await _artistBatchQueryService.QueryAsync();

            if(artistResult.IsFaulted)
            {
                incrementCountCallback(artistItemsQueried);
                continue;
            }

            // loop through results, assigning artist genres to artists
            foreach (var result in artistResult.Value.Artists)
            {
                incrementCountCallback(1);

                var artist = viewModel.Artists.Items[result.Id];

                artist.Genres = result.Genres;
                _stalkModelTransformer.RegisterGenre(viewModel, artist);
            }
        }
    }
}
