using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyStalker.Data.Migrations;

public partial class TracksView : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"

create or alter view dbo.vTracks as

select
    a.ArtistId,
    a.ArtistName,
    a.Genres,
    t.id,
    t.[Name],
    t.Popularity,
    t.Danceability,
    t.Energy,
    t.[Key],
    t.Loudness,
    t.Mode,
    t.Speechiness,
    t.Acousticness,
    t.Instrumentalness,
    t.Liveness,
    t.Valence,
    t.Tempo,
    t.DurationMs,
    t.TimeSignature,
    t.AddedDate
from
    dbo.Artists a
    inner join dbo.Tracks t on a.ArtistId = t.ArtistId;

");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("drop view if exists dbo.vTracks;");
    }
}
