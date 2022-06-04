using Microsoft.EntityFrameworkCore.Migrations;

namespace SpotifyStalker.Data.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Artists",
            columns: table => new
            {
                ArtistId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                ArtistName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Popularity = table.Column<int>(type: "int", nullable: false),
                Genres = table.Column<string>(type: "nvarchar(2040)", maxLength: 2040, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Artists", x => x.ArtistId);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Artists");
    }
}
