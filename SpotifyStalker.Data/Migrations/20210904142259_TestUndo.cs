using Microsoft.EntityFrameworkCore.Migrations;

namespace SpotifyStalker.Data.Migrations;

public partial class TestUndo : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Test",
            table: "Artists");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "Test",
            table: "Artists",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }
}
