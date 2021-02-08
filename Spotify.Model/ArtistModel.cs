using Spotify.Interface;
using Spotify.Object;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Spotify.Model
{
    public class ArtistModel : Artist, IApiRequestObject
    {
        [JsonIgnore]
        public string UrlTemplate => "artists/{Id}";

        public ConcurrentDictionary<string, Track>? Tracks { get; set; }
    }

    public class ArtistModelCollection : IApiBatchRequestObject
    {
        [JsonIgnore]
        public string UrlBatch => "artists?ids=";

        [JsonPropertyName("artists")]
        public IEnumerable<ArtistModel> Artists { get; set; }
    }
}
