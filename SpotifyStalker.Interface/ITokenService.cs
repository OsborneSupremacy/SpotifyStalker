using Spotify.Object;

namespace SpotifyStalker.Interface;

public interface ITokenService
{
    Task<Token> GetAsync();
}
