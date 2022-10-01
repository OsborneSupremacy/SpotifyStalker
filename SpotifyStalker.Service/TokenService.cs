using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Spotify.Object;

namespace SpotifyStalker.Service;

public class TokenService : ITokenService
{
    private readonly ILogger<ITokenService> _logger;

    private readonly IHttpFormPostService _httpFormPostService;

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly SpotifyApiSettings _spotifyApiSettings;

    private Token _token;

    private Task<Token> _tokenTask;

    private readonly object _tokenTaskLocker = new object();

    public TokenService(
        ILogger<ITokenService> logger,
        IOptions<SpotifyApiSettings> settings,
        IHttpFormPostService httpFormPostService,
        IHttpClientFactory httpClientFactory,
        IDateTimeProvider dateTimeProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _spotifyApiSettings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _httpFormPostService = httpFormPostService ?? throw new ArgumentNullException(nameof(httpFormPostService));
        _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
    }

    public async Task<Token> GetAsync()
    {
        if (_token != null && _token.ExpirationDate > _dateTimeProvider.GetCurrentDateTime())
        {
            _logger.LogDebug("Valid token found");
            return _token;
        }

        lock (_tokenTaskLocker)
        {
            switch (_tokenTask?.Status ?? TaskStatus.RanToCompletion)
            {
                case TaskStatus.RanToCompletion:
                case TaskStatus.Canceled:
                case TaskStatus.Faulted: // create new task for these statuses
                    _tokenTask = GetTokenAsync();
                    break;
                default: // use current task
                    break;
            }
        }
        return await _tokenTask;
    }

    private async Task<Token> GetTokenAsync()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", $"{GetUserNameAndPasswordBase64(_spotifyApiSettings.ApiKeys.ClientId, _spotifyApiSettings.ApiKeys.ClientSecret)}");

        var startTime = _dateTimeProvider.GetCurrentDateTime();

        var response = await _httpFormPostService.PostFormAsync(client, _spotifyApiSettings.TokenUrl,
            new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            }
        );

        _token = JsonSerializer.Deserialize<Token>(response);

        _token.ExpirationDate = startTime.AddSeconds(_token.ExpiresIn);
        return _token;
    }

    public string GetUserNameAndPasswordBase64(string clientID, string clientSecret)
    {
        var apiKeysBytes = Encoding.ASCII.GetBytes($"{clientID}:{clientSecret}");
        return Convert.ToBase64String(apiKeysBytes);
    }
}
