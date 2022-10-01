using SpotifyStalker.Model;

namespace SpotifyStalker.Interface;

public interface IMetricProvider
{
    Task<IEnumerable<Metric>> GetAllAsync();
}
