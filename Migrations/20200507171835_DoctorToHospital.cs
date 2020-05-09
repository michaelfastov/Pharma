using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class DoctorToHospital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorToHospitals",
                columns: table => new
                {
                    DoctorToHospitalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(nullable: false),
                    HospitalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorToHospitals", x => x.DoctorToHospitalId);
                    table.ForeignKey(
                        name: "FK_DoctorToHospitals_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorToHospitals_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorToHospitals_DoctorId",
                table: "DoctorToHospitals",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorToHospitals_HospitalId",
                table: "DoctorToHospitals",
                column: "HospitalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorToHospitals");
        }
    }
}
