using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyStalker.Data.Migrations;

public partial class SearchProcUpdate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"

create or alter proc dbo.pSearch
    @SearchTerm nvarchar(255)

as

select
    ArtistId,
    ArtistName,
    Genres,
    id,
    [Name],
    Popularity,
    Danceability,
    Energy,
    [Key],
    Loudness,
    Mode,
    Speechiness,
    Acousticness,
    Instrumentalness,
    Liveness,
    Valence,
    Tempo,
    DurationMs,
    TimeSignature,
    AddedDate
from
    dbo.vTracks
where
    ArtistName like '%' + @searchTerm + '%'

union

select
    ArtistId,
    ArtistName,
    Genres,
    id,
    [Name],
    Popularity,
    Danceability,
    Energy,
    [Key],
    Loudness,
    Mode,
    Speechiness,
    Acousticness,
    Instrumentalness,
    Liveness,
    Valence,
    Tempo,
    DurationMs,
    TimeSignature,
    AddedDate
from
    dbo.vTracks
where
    [Name] like '%' + @searchTerm + '%'

");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"

create or alter proc dbo.pSearch
    @SearchTerm nvarchar(255)

as

select
    ArtistId,
    ArtistName,
    Genres,
    id,
    [Name],
    Popularity,
    Danceability,
    Energy,
    [Key],
    Loudness,
    Mode,
    Speechiness,
    Acousticness,
    Instrumentalness,
    Liveness,
    Valence,
    Tempo,
    DurationMs,
    TimeSignature,
    AddedDate
from
    dbo.vTracks
where
    ArtistName like '%' + @searchTerm + '%'


select
    ArtistId,
    ArtistName,
    Genres,
    id,
    [Name],
    Popularity,
    Danceability,
    Energy,
    [Key],
    Loudness,
    Mode,
    Speechiness,
    Acousticness,
    Instrumentalness,
    Liveness,
    Valence,
    Tempo,
    DurationMs,
    TimeSignature,
    AddedDate
from
    dbo.vTracks
where
    [Name] like '%' + @searchTerm + '%'

");
    }
}
