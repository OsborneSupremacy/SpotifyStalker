using System;

namespace SpotifyStalker.Service;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IApiRequestService))]
public class ApiRequestService : IApiRequestService
{
    private readonly ILogger<ApiRequestService> _logger;

    private readonly IAuthorizedHttpClientFactory _httpClientFactory;

    public ApiRequestService(
        ILogger<ApiRequestService> logger,
        IAuthorizedHttpClientFactory httpClientFactory
    )
    {
        _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        _httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task<Outcome<T>> GetAsync<T>(string url)
    {
        var responseMessage = await TryGetAsync<T>(url);

        while (
            responseMessage.IsFaulted
            && responseMessage.Exception is RequestException rex
            && rex.Retry)
        {
            await Task.Delay((int)rex.WaitMs);
            return await GetAsync<T>(url);
        };

        if (responseMessage.IsFaulted)
            return new Outcome<T>(responseMessage.Exception);

        return await ReadAndDeserializeAsync<T>(responseMessage.Value);
    }

    protected async Task<T> ReadAndDeserializeAsync<T>(HttpResponseMessage message)
    {
        var stringResponse = await message.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(stringResponse);
    }

    protected async Task<Outcome<HttpResponseMessage>> TryGetAsync<T>(string url)
    {
        using var httpClient = await _httpClientFactory.CreateClientAsync();
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

        _logger.LogDebug("Querying {uri}", url);
        return await ExecuteRequestAsync(httpClient, httpRequest);
    }

    protected async Task<Outcome<HttpResponseMessage>> ExecuteRequestAsync(
        HttpClient httpClient,
        HttpRequestMessage httpRequest
        )
    {
        var response = await httpClient.SendAsync(httpRequest);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogDebug("Success response received");
            return response;
        }

        _logger.LogDebug("Success response not received");

        // failed response at this point
        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                _logger.LogDebug("Not found");
                return new Outcome<HttpResponseMessage>(new RequestException(RequestStatus.NotFound));

            case HttpStatusCode.TooManyRequests: // rate limit hit. Retry
            case HttpStatusCode.ServiceUnavailable:
                var waitMs = response.Headers?.RetryAfter?.Delta?.TotalMilliseconds ?? 5000;
                _logger.LogDebug($"Rate limit hit. Retry in {waitMs} milliseconds.");
                return new Outcome<HttpResponseMessage>(new RequestException(RequestStatus.Retry, true, waitMs));

            default:
                try
                {
                    response.EnsureSuccessStatusCode(); // do this so an exception can be generated and logged
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Request failed");
                    return new Outcome<HttpResponseMessage>(ex);
                }
                break;
        }

        return new Outcome<HttpResponseMessage>(new RequestException(RequestStatus.Failed));

    }
}
