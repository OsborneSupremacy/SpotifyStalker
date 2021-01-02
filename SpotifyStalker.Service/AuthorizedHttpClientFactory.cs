using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Headers;
using SpotifyStalker.Interface;

namespace SpotifyStalker.Service
{
    public class AuthorizedHttpClientFactory : IAuthorizedHttpClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ITokenService _tokenService;

        public AuthorizedHttpClientFactory(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task<HttpClient> CreateClientAsync()
        {
            var authToken = await _tokenService.GetAsync();

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.AccessToken);

            return client;
        }
    }
}
