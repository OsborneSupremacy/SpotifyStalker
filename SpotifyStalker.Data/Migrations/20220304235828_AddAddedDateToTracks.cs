using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyStalker.Data.Migrations;

public partial class AddAddedDateToTracks : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "AddedDate",
            table: "Tracks",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AddedDate",
            table: "Tracks");
    }
}
