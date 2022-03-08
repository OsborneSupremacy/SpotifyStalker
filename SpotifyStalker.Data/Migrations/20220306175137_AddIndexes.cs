using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyStalker.Data.Migrations
{
    public partial class AddIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tracks_Name",
                table: "Tracks",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_ArtistName",
                table: "Artists",
                column: "ArtistName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tracks_Name",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Artists_ArtistName",
                table: "Artists");
        }
    }
}
