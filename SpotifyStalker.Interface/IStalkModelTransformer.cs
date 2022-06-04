using System.Collections.Generic;
using System.Threading.Tasks;
using Spotify.Interface;
using Spotify.Model;
using Spotify.Object;
using SpotifyStalker.Model;

namespace SpotifyStalker.Interface;

public interface IStalkModelTransformer
{
    StalkModel RegisterTrack(StalkModel stalkModel, PlaylistModel playlistModel, Track track);

    StalkModel RegisterGenre(StalkModel stalkModel, ArtistModel artist);

    StalkModel RegisterAudioFeature(StalkModel stalk, AudioFeaturesModel audioFeatures);

    Task<StalkModel> ResetAsync(StalkModel stalkModel);


    StalkModel IncrementCount<T>(StalkModel stalkModel) where T : ISpotifyStandardObject;

    List<T> GetItems<T>(StalkModel stalkModel) where T : ISpotifyStandardObject;

    StalkModel RegisterPlaylists(StalkModel stalkModel, IEnumerable<Playlist> playlists);
}
