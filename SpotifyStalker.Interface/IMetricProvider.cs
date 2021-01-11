using SpotifyStalker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface IMetricProvider
    {
        Task<IEnumerable<Metric>> GetAllAsync();
    }
}
