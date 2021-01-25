using Spotify.Model;
using Spotify.Object;
using System.Collections.Generic;

namespace SpotifyStalker.Model
{
    public class FindPersonModel
    {
        public string Keyword { get; set; }

        public RequestStatus SearchStatus { get; set; }

        public List<Playlist> Playlists { get; set; }



    }
}
