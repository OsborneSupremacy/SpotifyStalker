using Spotify.Object;

namespace SpotifyStalker.Model;

public record ExampleTrack
{
    public ExampleTrack(
        Metric metric,
        double metricValue,
        Track track,
        double markerPosition
        )
    {
        Metric = metric ?? throw new ArgumentNullException(nameof(metric));
        MetricValue = metricValue;
        Track = track ?? throw new ArgumentNullException(nameof(track));
        MarkerPosition = markerPosition;
    }

    public Metric Metric { get; }

    public double MetricValue { get; }

    public Track Track { get; }

    public double MarkerPosition { get; }

    public string FormattedValue => 
        MetricValue.ToString(Metric.FormatString);
}
