using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpotifyStalker.Data.Migrations
{
    public partial class AddArtistQueryQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Genres",
                table: "Artists",
                type: "nvarchar(max)",
                maxLength: 4080,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2040)",
                oldMaxLength: 2040);

            migrationBuilder.CreateTable(
                name: "ArtistQueryLogs",
                columns: table => new
                {
                    SearchTerm = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    QueriedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResultCount = table.Column<int>(type: "int", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistQueryLogs", x => x.SearchTerm);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artists_ArtistId",
                table: "Artists",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistQueryLogs_SearchTerm",
                table: "ArtistQueryLogs",
                column: "SearchTerm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistQueryLogs");

            migrationBuilder.DropIndex(
                name: "IX_Artists_ArtistId",
                table: "Artists");

            migrationBuilder.AlterColumn<string>(
                name: "Genres",
                table: "Artists",
                type: "nvarchar(2040)",
                maxLength: 2040,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 4080);
        }
    }
}
