using System.Reflection.Metadata;
using Spotify.Object;

namespace SpotifyStalker.Model;

public record Metric : ISpotifyStandardObject
{
    public string Name { get; set; }

    public string Id => Name;

    public int Priority;

    public string Content { get; set; }

    public string ImageFile { get; set; }

    public string Unit { get; set; }

    public double Min { get; set; }

    public double Max { get; set; }

    public double NominalMin { get; set; }

    public double NominalMax { get; set; }

    public double? Average { get; set; }

    public string FormattedAverage => (Average ?? 0).ToString(FormatString);

    public double GlobalAverage { get; init; }

    public string FormattedGlobalAverage => GlobalAverage.ToString(FormatString);

    public double? MarkerPosition { get; set; }

    public Func<AudioFeatures, double?> Field { get; set; }

    public string FormatString { get; set; }

    public ExampleTrack? Winner { get; set; }

    public ExampleTrack? Loser { get; set; }
}
