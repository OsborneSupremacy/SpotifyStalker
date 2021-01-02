using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading;
using SpotifyStalker.Interface;
using System.Net;
using SpotifyStalker.Model;

namespace SpotifyStalker.Service
{
    public class ApiRequestService : IApiRequestService
    {
        private readonly ILogger<IApiRequestService> _logger;

        private readonly IAuthorizedHttpClientFactory _httpClientFactory;

        public ApiRequestService(ILogger<IApiRequestService> logger, IAuthorizedHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<(RequestStatus RequestStatus, T)> GetAsync<T>(string url)
        {
            var keepTrying = true;
            var requestStatus = RequestStatus.Default;

            while (keepTrying)
            {
                using var httpClient = await _httpClientFactory.CreateClientAsync();
                using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

                var apiResponse = await ExecuteRequestAsync(httpClient, httpRequest);
                requestStatus = apiResponse.RequestStatus;

                if (requestStatus == RequestStatus.Retry)
                {
                    Thread.Sleep((int)apiResponse.WaitMs);
                    continue;
                };

                // success -- read response body
                var stringResponse = await apiResponse.HttpResponseMessage.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<T>(stringResponse);

                return (requestStatus, deserialized);
            }

            return (requestStatus, default);
        }

        protected struct ApiResponse
        {
            public RequestStatus RequestStatus;
            public double WaitMs;
            public HttpResponseMessage HttpResponseMessage;
        }

        protected async Task<ApiResponse> ExecuteRequestAsync(
            HttpClient httpClient,
            HttpRequestMessage httpRequest
            )
        {
            var apiResponse = new ApiResponse()
            {
                RequestStatus = RequestStatus.Default
            };

            var response = await httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Success response received");
                apiResponse.RequestStatus = RequestStatus.Success;
                apiResponse.HttpResponseMessage = response;
                return apiResponse;
            }

            // failed response at this point
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    apiResponse.RequestStatus = RequestStatus.NotFound;
                    return apiResponse;
                case HttpStatusCode.TooManyRequests: // rate limit hit. Retry
                case HttpStatusCode.ServiceUnavailable:
                    apiResponse.RequestStatus = RequestStatus.Retry;
                    apiResponse.WaitMs = response.Headers?.RetryAfter?.Delta?.TotalMilliseconds ?? (double)5000;
                    _logger.LogInformation($"Rate limit hit. Retry in {apiResponse.WaitMs} milliseconds.");
                    return apiResponse;
            }

            // don't retry
            try
            {
                response.EnsureSuccessStatusCode(); // do this an exception can be generated and logged
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Request failed");
            }

            return apiResponse;
        }
    }
}
