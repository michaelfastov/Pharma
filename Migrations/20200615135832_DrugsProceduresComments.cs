using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharma.Migrations
{
    public partial class DrugsProceduresComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Procedures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Drugs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Procedures");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Drugs");
        }
    }
}
