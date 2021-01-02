using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading;
using SpotifyStalker.Interface;

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

        public async Task<T> GetAsync<T>(string url)
        {
            var keepTrying = true;

            while (keepTrying)
            {
                using var httpClient = await _httpClientFactory.CreateClientAsync();
                using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

                var apiResponse = await ExecuteRequestAsync(httpClient, httpRequest);

                keepTrying = (!apiResponse.Success && apiResponse.Retry);
                if (!apiResponse.Success)
                {
                    Thread.Sleep((int)apiResponse.WaitMs);
                    continue;
                };

                // success -- read response body
                var stringResponse = await apiResponse.HttpResponseMessage.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<T>(stringResponse);

                return deserialized;
            }

            return default;
        }

        protected struct ApiResponse
        {
            public bool Success;
            public bool Retry;
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
                Success = false,
                Retry = false
            };

            var response = await httpClient.SendAsync(httpRequest);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Success response received");
                apiResponse.Success = true;
                apiResponse.HttpResponseMessage = response;
                return apiResponse;
            }

            // failed response at this point
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.TooManyRequests: // rate limit hit. Retry
                case System.Net.HttpStatusCode.ServiceUnavailable:
                    apiResponse.Retry = true;
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
