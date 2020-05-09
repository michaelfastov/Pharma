using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class DoctorReceptionPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ReceptionPrice",
                table: "Doctors",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceptionPrice",
                table: "Doctors");
        }
    }
}
