using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class ReceptionDayOfWeek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Receptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Receptions");
        }
    }
}
