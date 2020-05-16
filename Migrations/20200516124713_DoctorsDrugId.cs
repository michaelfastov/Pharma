using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class DoctorsDrugId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Drugs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_DoctorId",
                table: "Drugs",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Doctors_DoctorId",
                table: "Drugs",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Doctors_DoctorId",
                table: "Drugs");

            migrationBuilder.DropIndex(
                name: "IX_Drugs_DoctorId",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Drugs");
        }
    }
}
