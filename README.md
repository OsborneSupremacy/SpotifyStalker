# SpotifyStalker

## SpotifyStalker2

This is the main project of this solution and should be the startup project (unless you want to gather data from Spotify [see `SpotifyStalker.ConsoleUi` below]).

### .TXT Lists

The files in SpotifyStalker.Service\Files\ are included in the output folder. These are lists used for "pick random" functions.

#### Artist List

This data was queried from Spotify using `SpotifyStalker.ConsoleUi`. It should be refreshed periodically.

Once the console app has finished querying artists, the data is available here:

```sql
select ArtistName from dbo.Artists order by Popularity desc, ArtistName;
```

#### Genre List

```sql
select
    g.Genre
from
    (
        select a.ArtistId, Genre = [value]
        from dbo.Artists a
        cross apply string_split(a.Genres, '|')
    ) g
group by
    g.Genre
order by
    count(g.ArtistId) desc;

```

#### Track Metrics

```sql
select
    TrackCount = count(m.Id),
    MaxDanceability = max(m.Danceability),
    AvgDanceability = avg(m.Danceability),
    MinDanceability = min(m.Danceability),
    MaxEnergy = max(m.Energy),
    AveEnergy = avg(m.Energy),
    MinEnergy = min(m.Energy),
    MaxLoudness = max(m.Loudness),
    AvgLoudness = avg(m.Loudness),
    MinLoudness = min(m.Loudness),
    MaxSpeechiness = max(m.Speechiness),
    AvgSpeechiness = avg(m.Speechiness),
    MinSpeechiness = min(m.Speechiness),
    MaxAcousticness = max(m.Acousticness),
    AvgAcousticness = avg(m.Acousticness),
    MinAcousticness = min(m.Acousticness),
    MaxInstrumentalness = max(m.Instrumentalness),
    AvgInstrumentalness = avg(m.Instrumentalness),
    MinInstrumentalness = min(m.Instrumentalness),
    MaxLiveness = max(m.Liveness),
    AvgLiveness = avg(m.Liveness),
    MinLiveness = min(m.Liveness),
    MaxValence = max(m.Valence),
    AvgValence = avg(m.Valence),
    MinValence = min(m.Valence),
    MaxTempo = max(m.Tempo),
    AvgTempo = avg(m.Tempo),
    MinTempo = min(m.Tempo)
from
    dbo.Tracks m;
```

## SpotifyStalker.ConsoleUi / SpotifyStalker.Data

These two projects are independent of the web application, `SpotifyStalker2`.

They can be used to query data from Spotify, saving it back to a SQL Server database for various purposes (e.g. populating the lists of artists and genres).


### Using SpotifyStalker.Data

Update appSettings.json with the connection string on your PC, or add it to your user secrets.

#### Deploying Database Updates

Use the .NET Core CLI to deploy database updates on your system.

Navigate to the root folder of SpotifyStalker.Data.

If the EF tools need to be installed, run this:

```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
```

To add a migration, run:

```
dotnet ef migrations add MigrationName
```

To apply the migrations, run:

```
dotnet ef database update
```

[Reference](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)

### Using SpotifyStalker.ConsoleUi

Once a database is created, set SpotifyStalker.ConsoleUi as your startup project, and run it. It will write data to your local SpotifyStalker database.
