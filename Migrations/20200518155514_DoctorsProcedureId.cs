using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class DoctorsProcedureId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Procedures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_DoctorId",
                table: "Procedures",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Doctors_DoctorId",
                table: "Procedures",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Doctors_DoctorId",
                table: "Procedures");

            migrationBuilder.DropIndex(
                name: "IX_Procedures_DoctorId",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Procedures");
        }
    }
}
