using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyStalker.ConsoleUi
{
    public class ArtistQueryService
    {
        private readonly ILogger<ArtistQueryService> _logger;

        private readonly IApiQueryService _apiQueryService;

        private readonly SpotifyApiSettings _spotifyApiSettings;

        private readonly string[] _searchCharacters =
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "!",
            "@",
            "#",
            "$",
            "%",
            "^",
            "&",
            "*",
            "(",
            ")",
            "+",
            "=",
            "?",
            " "
        };

        public ArtistQueryService(
            ILogger<ArtistQueryService> logger,
            IOptions<SpotifyApiSettings> settings,
            IApiQueryService apiQueryService
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _spotifyApiSettings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _apiQueryService = apiQueryService ?? throw new ArgumentNullException(nameof(apiQueryService));
        }

        public async Task ExecuteAsync()
        {
            _logger.LogDebug("Querying Artists");

            var artists = new Dictionary<string, Artist>();

            foreach(var searchTerm in _searchCharacters)
                artists = await QueryArtistsUsingSearchTermsAsync(searchTerm, artists);

            foreach (var artist in artists.OrderBy(x => x.Value.Name))
                Console.WriteLine($"{artist.Key} - {artist.Value.Name}");
        }

        public async Task<Dictionary<string, Artist>> QueryArtistsUsingSearchTermsAsync(string searchTerms, Dictionary<string, Artist> artists)
        {
            _logger.LogDebug("Querying Artists with {searchTerms}", searchTerms);

            var itemsQueried = 0;
            var totalItems = 1;
            bool firstIteration = true;

            while (itemsQueried < totalItems)
            {
                var (result, resultModel) = await _apiQueryService.QueryAsync<ArtistSearchResultModel>(searchTerms, _spotifyApiSettings.Limits.Search.Limit, itemsQueried);
                itemsQueried += _spotifyApiSettings.Limits.Search.Limit;

                if (result != Model.RequestStatus.Success)
                    break;

                if (firstIteration)
                {
                    totalItems = resultModel.Artists.Total;
                    if (totalItems > _spotifyApiSettings.Limits.Search.MaximumOffset)
                        totalItems = _spotifyApiSettings.Limits.Search.MaximumOffset;
                    _logger.LogDebug("Total Items: {totalItems}", totalItems);
                    firstIteration = false;
                }

                foreach (var item in resultModel.Artists.Items)
                    artists.TryAdd(item.Id, item);
            }

            return artists;
        }
    }
}
