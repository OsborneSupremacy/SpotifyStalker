using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface;

public interface IGenreProvider
{
    Task<IEnumerable<string>> GetAsync();
}
