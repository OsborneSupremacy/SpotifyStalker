using System.Reflection;

namespace SpotifyStalker.Service;

[ServiceLifetime(ServiceLifetime.Scoped)]
[RegistrationTarget(typeof(IAudioFeaturesQueryService))]
public class AudioFeaturesQueryService : IAudioFeaturesQueryService
{
    private readonly IApiBatchQueryService<AudioFeaturesModelCollection> _audioFeaturesQueryService;

    private readonly IStalkModelTransformer _stalkModelTransformer;

    public AudioFeaturesQueryService(
        IApiBatchQueryService<AudioFeaturesModelCollection> audioFeaturesQueryService,
        IStalkModelTransformer stalkModelTransformer
        )
    {
        _audioFeaturesQueryService = audioFeaturesQueryService ?? throw new ArgumentNullException(nameof(audioFeaturesQueryService));
        _stalkModelTransformer = stalkModelTransformer ?? throw new ArgumentNullException(nameof(stalkModelTransformer));
    }

    public async Task QueryAsync(
        StalkModel viewModel,
        Action<string> statusUpdateCallback,
        Action<int> incrementCountCallback
        )
    {
        statusUpdateCallback("Looking up audio features");

        foreach (var track in viewModel.Tracks.Items)
        {
            if (!string.IsNullOrEmpty(track.Value.Id))
                _audioFeaturesQueryService.AddToQueue(track.Value.Id);
        }

        while (!_audioFeaturesQueryService.QueueIsEmpty())
        {
            var (audioFeaturesResult, audioFeaturesQueriedCount) = await _audioFeaturesQueryService.QueryAsync();

            if(audioFeaturesResult.IsFaulted)
            {
                incrementCountCallback(audioFeaturesQueriedCount);
                continue;
            }

            foreach (var result in audioFeaturesResult.Value.AudioFeaturesList)
            {
                incrementCountCallback(1);
                viewModel = _stalkModelTransformer.RegisterAudioFeature(viewModel, result);
            }
        }
    }
}
