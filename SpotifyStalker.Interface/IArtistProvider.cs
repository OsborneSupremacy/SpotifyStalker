using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface IArtistProvider
    {
        Task<IEnumerable<string>> GetAsync();
    }
}
