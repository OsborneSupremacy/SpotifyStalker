using Spotify.Object;
using SpotifyStalker.Interface;
using SpotifyStalker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyStalker.Service
{
    public class MetricProvider : IMetricProvider
    {
        public async Task<IEnumerable<Metric>> GetAllAsync()
        {
            return await Task.Run(() =>
            {

                return new List<Metric>() {

                    new Metric()
                    {
                        Name = "Danceability",
                        Content = "Danceability describes how suitable a track is for dancing based on a combination of musical elements including tempo, rhythm stability, beat strength, and overall regularity.A value of 0.0 is least danceable and 1.0 is most danceable.",
                        Unit = "",
                        Min = 0,
                        Max = 1,
                        NominalMin = 0,
                        NominalMax = 1,
                        Field = (AudioFeatures x) => x.Danceability,
                        FormatString = "0.0000",
                        ImageFile = "danceability.png",
                        Priority = 40
                    },

                    new Metric()
                    {
                        Name = "Energy",
                        Content = "Energy is a measure from 0.0 to 1.0 and represents a perceptual measure of intensity and activity. Typically, energetic tracks feel fast, loud, and noisy. For example, death metal has high energy, while a Bach prelude scores low on the scale. Perceptual features contributing to this attribute include dynamic range, perceived loudness, timbre, onset rate, and general entropy.",
                        Unit = "",
                        Min = 0,
                        Max = 1,
                        NominalMin = 0,
                        NominalMax = 1,
                        Field = (AudioFeatures x) => x.Energy,
                        FormatString = "0.0000",
                        ImageFile = "energy.png",
                        Priority = 30
                    },

                    new Metric()
                    {
                        Name = "Loudness",
                        Content = "The overall loudness of a track in decibels (dB). Loudness values are averaged across the entire track and are useful for comparing relative loudness of tracks. Loudness is the quality of a sound that is the primary psychological correlate of physical strength (amplitude). Values typical range between -60 and 0 db.",
                        Unit = "dB",
                        Min = -49,
                        Max = 2,
                        NominalMin = -60,
                        NominalMax = 0,
                        Field = (AudioFeatures x) => x.Loudness,
                        FormatString = "0.0000 dB",
                        ImageFile = "loudness.png",
                        Priority = 10
                    },

                    new Metric()
                    {
                        Name = "Valence",
                        Content = "A measure from 0.0 to 1.0 describing the musical positiveness conveyed by a track. Tracks with high valence sound more positive (e.g. happy, cheerful, euphoric), while tracks with low valence sound more negative (e.g. sad, depressed, angry).",
                        Unit = "",
                        Min = 0,
                        Max = 1,
                        NominalMin = 0,
                        NominalMax = 1,
                        Field = (AudioFeatures x) => x.Valence,
                        FormatString = "0.0000",
                        ImageFile = "valence.png",
                        Priority = 20
                    },

                    new Metric()
                    {
                        Name = "Speechiness",
                        Content = "Speechiness detects the presence of spoken words in a track. The more exclusively speech-like the recording (e.g. talk show, audio book, poetry), the closer to 1.0 the attribute value. Values above 0.66 describe tracks that are probably made entirely of spoken words. Values between 0.33 and 0.66 describe tracks that may contain both music and speech, either in sections or layered, including such cases as rap music. Values below 0.33 most likely represent music and other non-speech-like tracks.",
                        Unit = "",
                        Min = 0,
                        Max = 1,
                        NominalMin = 0,
                        NominalMax = 1,
                        Field = (AudioFeatures x) => x.Speechiness,
                        FormatString = "0.0000",
                        ImageFile = "speechiness.png",
                        Priority = 90
                    },

                    new Metric()
                    {
                        Name = "Acousticness",
                        Content = "A confidence measure from 0.0 to 1.0 of whether the track is acoustic. 1.0 represents high confidence the track is acoustic.",
                        Unit = "",
                        Min = 0,
                        Max = 1,
                        NominalMin = 0,
                        NominalMax = 1,
                        Field = (AudioFeatures x) => x.Acousticness,
                        FormatString = "0.0000",
                        ImageFile = "acousticness.png",
                        Priority = 80
                    },

                    new Metric()
                    {
                        Name = "Instrumentalness",
                        Content = "Predicts whether a track contains no vocals. “Ooh” and “aah” sounds are treated as instrumental in this context. Rap or spoken word tracks are clearly “vocal”. The closer the instrumentalness value is to 1.0, the greater likelihood the track contains no vocal content. Values above 0.5 are intended to represent instrumental tracks, but confidence is higher as the value approaches 1.0.",
                        Unit = "",
                        Min = 0,
                        Max = 1,
                        NominalMin = 0,
                        NominalMax = 1,
                        Field = (AudioFeatures x) => x.Instrumentalness,
                        FormatString = "0.0000",
                        ImageFile = "instrumentalness.png",
                        Priority = 60
                    },

                    new Metric()
                    {
                        Name = "Liveness",
                        Content = "Detects the presence of an audience in the recording. Higher liveness values represent an increased probability that the track was performed live. A value above 0.8 provides strong likelihood that the track is live.",
                        Unit = "",
                        Min = 0,
                        Max = 1,
                        NominalMin = 0,
                        NominalMax = 1,
                        Field = (AudioFeatures x) => x.Liveness,
                        FormatString = "0.0000",
                        ImageFile = "liveness.png",
                        Priority = 70
                    },

                    new Metric()
                    {
                        Name = "Tempo",
                        Content = "The overall estimated tempo of a track in beats per minute (BPM). In musical terminology, tempo is the speed or pace of a given piece and derives directly from the average beat duration.",
                        Unit = "BPM",
                        Min = 0,
                        Max = 250,
                        NominalMin = 0,
                        NominalMax = 250,
                        Field = (AudioFeatures x) => x.Tempo,
                        FormatString = "0.0000 BPM",
                        ImageFile = "tempo.png",
                        Priority = 50
                    }
                };
            });
        }
    }
}
