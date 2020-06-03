using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class ReceptionDataSignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Receptions");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "Receptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Receptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Receptions");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Receptions");

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Receptions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
