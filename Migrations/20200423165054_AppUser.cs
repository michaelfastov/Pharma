using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class AppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityId",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityId",
                table: "Doctors",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FacebookId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IdentityId",
                table: "Patients",
                column: "IdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_IdentityId",
                table: "Doctors",
                column: "IdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_IdentityId",
                table: "Doctors",
                column: "IdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_IdentityId",
                table: "Patients",
                column: "IdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_IdentityId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_IdentityId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_IdentityId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_IdentityId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "FacebookId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "AspNetUsers");
        }
    }
}
