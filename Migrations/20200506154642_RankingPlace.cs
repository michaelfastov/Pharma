using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class RankingPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankingPlace",
                table: "DoctorToDoctorRatings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RankingPlace",
                table: "DoctorToDoctorRatings");
        }
    }
}
