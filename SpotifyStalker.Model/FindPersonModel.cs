﻿using Spotify.Object;

namespace SpotifyStalker.Model;

public class FindPersonModel
{
    public string Keyword { get; set; }

    public RequestStatus SearchStatus { get; set; }

    public List<Playlist> Playlists { get; set; }

    public bool NoPlayListsFound =>
        SearchStatus == RequestStatus.Success && !(Playlists?.Any() ?? false);
}
