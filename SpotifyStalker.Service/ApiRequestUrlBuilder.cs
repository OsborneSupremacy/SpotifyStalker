using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spotify.Interface;
using Spotify.Model;
using SpotifyStalker.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SpotifyStalker.Service
{
    public class ApiRequestUrlBuilder : IApiRequestUrlBuilder
    {
        private readonly ILogger<ApiRequestUrlBuilder> _logger;

        private readonly SpotifyApiSettings _spotifyApiSettings;

        public ApiRequestUrlBuilder(
            ILogger<ApiRequestUrlBuilder> logger,
            IOptions<SpotifyApiSettings> settings
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _spotifyApiSettings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public string Build<T>(string id) where T : IApiRequestObject, new() =>
            Build<T>(new KeyValuePair<string, string>("Id", id));

        public string Build<T>(string id, int limit) where T : IApiRequestObject, new() =>
            Build<T>(
                new KeyValuePair<string, string>("Id", id),
                new KeyValuePair<string, string>("Limit", limit.ToString())
            );

        public string Build<T>(string id, int limit, int offset) where T : IApiRequestObject, new() =>
            Build<T>(
                new KeyValuePair<string, string>("Id", id),
                new KeyValuePair<string, string>("Limit", limit.ToString()),
                new KeyValuePair<string, string>("Offset", offset.ToString())
            );

        public string Build<T>(params KeyValuePair<string, string>[] substitutions) where T : IApiRequestObject, new()
        {
            var u = new StringBuilder(_spotifyApiSettings.SpotifyBaseUrl);
            u.Append($"/{new T().UrlTemplate}");

            foreach (var sub in substitutions)
                u.Replace("{" + sub.Key + "}", HttpUtility.UrlEncode(sub.Value));

            var url = u.ToString();

            if(url.Contains("{"))
                throw new FormatException($"Url contains un-replaced placeholders, `{url}`");

            return url;
        }

        public string BuildBatch<T>(IEnumerable<string> ids) where T : IApiBatchRequestObject, new()
        {
            var u = new StringBuilder(_spotifyApiSettings.SpotifyBaseUrl);
            u.Append($"/{new T().UrlBase}{string.Join(",",ids)}");

            var url = u.ToString();
            return url;
        }
    }
}
