using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class HospitalName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Hospitals",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Hospitals");
        }
    }
}
