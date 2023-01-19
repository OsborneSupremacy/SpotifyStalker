namespace SpotifyStalker.Interface;

public interface IMetricProvider
{
    IEnumerable<Metric> GetAll();
}
