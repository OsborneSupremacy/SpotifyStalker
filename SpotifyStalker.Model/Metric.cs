using Spotify.Interface;
using Spotify.Object;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpotifyStalker.Model
{
    public class Metric : ISpotifyStandardObject
    {
        public string Name { get; set; }

        public string Id  => Name;

        public string Content { get; set; }

        public string ImageFile { get; set; }

        public string Unit { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public double? NominalMin { get; set; }

        public double? NominalMax { get; set; }

        public double? Value { get; set; }

        public string FormattedValue => (Value ?? 0).ToString(FormatString);

        public double? MarkerPercentage { get; set; }

        public Func<AudioFeatures, double?> Field { get; set; }

        public string FormatString { get; set; }
    }
}
