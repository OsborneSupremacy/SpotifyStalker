using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyStalker.Model;

namespace SpotifyStalker.Interface;

public interface IMetricProvider
{
    Task<IEnumerable<Metric>> GetAllAsync();
}
