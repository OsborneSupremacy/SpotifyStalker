using Spotify.Object;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface ITokenService
    {
        Task<Token> GetAsync();
    }
}
