using System;
using Spotify.Interface;
using Spotify.Object;

namespace SpotifyStalker.Model;

public class Metric : ISpotifyStandardObject
{
    public string Name { get; set; }

    public string Id => Name;

    public int Priority;

    public string Content { get; set; }

    public string ImageFile { get; set; }

    public string Unit { get; set; }

    public double? Min { get; set; }

    public double? Max { get; set; }

    public double? NominalMin { get; set; }

    public double? NominalMax { get; set; }

    public double? Average { get; set; }

    public string FormattedAverage => (Average ?? 0).ToString(FormatString);

    public double? MarkerPosition { get; set; }

    public Func<AudioFeatures, double?> Field { get; set; }

    public string FormatString { get; set; }

    public (double MetricValue, Track Track)? Winner { get; set; }

    public (double MetricValue, Track Track)? Loser { get; set; }
}
