using Microsoft.EntityFrameworkCore.Migrations;

namespace SpotifyStalker.Data.Migrations
{
    public partial class Track : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ArtistId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Popularity = table.Column<double>(type: "float", nullable: true),
                    Danceability = table.Column<double>(type: "float", nullable: true),
                    Energy = table.Column<double>(type: "float", nullable: true),
                    Key = table.Column<double>(type: "float", nullable: true),
                    Loudness = table.Column<double>(type: "float", nullable: true),
                    Mode = table.Column<double>(type: "float", nullable: true),
                    Speechiness = table.Column<double>(type: "float", nullable: true),
                    Acousticness = table.Column<double>(type: "float", nullable: true),
                    Instrumentalness = table.Column<double>(type: "float", nullable: true),
                    Liveness = table.Column<double>(type: "float", nullable: true),
                    Valence = table.Column<double>(type: "float", nullable: true),
                    Tempo = table.Column<double>(type: "float", nullable: true),
                    DurationMs = table.Column<double>(type: "float", nullable: true),
                    TimeSignature = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_ArtistId",
                table: "Tracks",
                column: "ArtistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tracks");
        }
    }
}
