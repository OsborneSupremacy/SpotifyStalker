﻿namespace SpotifyStalker.Service;

[ServiceLifetime(ServiceLifetime.Scoped)]
[RegistrationTarget(typeof(IApiBatchQueryService<>))]
public class ApiBatchQueryService<T> : IApiBatchQueryService<T> where T : IApiBatchRequestObject, new()
{
    private readonly ILogger<IApiRequestService> _logger;

    private readonly SpotifyApiSettings _spotifyApiSettings;

    private readonly IApiRequestUrlBuilder _apiRequestUrlBuilder;

    private readonly IApiRequestService _apiRequestService;

    private readonly ConcurrentQueue<string> _queuedItems;

    public ApiBatchQueryService(
        ILogger<IApiRequestService> logger,
        IApiRequestUrlBuilder apiRequestUrlBuilder,
        IApiRequestService apiRequestService,
        SpotifyApiSettings settings
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiRequestUrlBuilder = apiRequestUrlBuilder ?? throw new ArgumentNullException(nameof(apiRequestUrlBuilder));
        _apiRequestService = apiRequestService ?? throw new ArgumentNullException(nameof(apiRequestService));
        _spotifyApiSettings = settings ?? throw new ArgumentNullException(nameof(settings));
        _queuedItems = new ConcurrentQueue<string>();
    }

    public void AddToQueue(string id) => _queuedItems.Enqueue(id);

    public bool QueueIsEmpty() => _queuedItems.IsEmpty;

    public async Task<(Outcome<T>, int)> QueryAsync()
    {
        int b = 0;
        var ids = new List<string>();

        while (b < _spotifyApiSettings.Limits.BatchSize && !QueueIsEmpty())
        {
            if (_queuedItems.TryDequeue(out string id))
            {
                ids.Add(id);
                b++;
            }
        }

        _logger.LogDebug($"Batch is ready to send. Item count {b}");

        var url = _apiRequestUrlBuilder.BuildBatch<T>(ids);

        return (await _apiRequestService.GetAsync<T>(url), b);
    }
}
