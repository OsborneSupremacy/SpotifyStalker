using Spotify.Interface;
using Spotify.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public double? MarkerPercentage { get; set; }

        public Func<AudioFeatures, double?> Field { get; set; }
    }

    public class MetricEnvelope
    {


        public MetricEnvelope()
        {

            Populated = false; 
        }

        public bool Populated { get; set; }


        public void Calculate(IEnumerable<AudioFeatures> audioFeatures)
        {
            Populated = true;
            Danceability.Value = calculateAverage(x => x.Danceability);
            Energy.Value = calculateAverage(x => x.Energy);
            Loudness.Value = calculateAverage(x => x.Loudness);
            Speechiness.Value = calculateAverage(x => x.Speechiness);
            Acousticness.Value = calculateAverage(x => x.Acousticness);
            Instrumentalness.Value = calculateAverage(x => x.Instrumentalness);
            Liveness.Value = calculateAverage(x => x.Liveness);
            Valence.Value = calculateAverage(x => x.Valence);
            Tempo.Value = calculateAverage(x => x.Tempo);

            var metrics = GetAllMetrics().ToList();

            foreach (var metric in metrics)
            {
                var mp = metric.Value / (metric.Max = metric.Min);
                if (mp < 0)
                    mp += 1.0;
                metric.MarkerPercentage = mp * 100;
            }

            double calculateAverage(Func<AudioFeatures, double?> field) =>
                audioFeatures
                    .ToList()
                    .Select(field)
                    .Where(x => x.HasValue)
                    .Select(x => x.Value)
                    .Average();
        }

    }

}
