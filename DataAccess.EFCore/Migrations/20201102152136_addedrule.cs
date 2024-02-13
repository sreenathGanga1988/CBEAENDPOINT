using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedrule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "ClaimCounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                table: "ClaimCounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "ClaimCounts");

            migrationBuilder.DropColumn(
                name: "StateName",
                table: "ClaimCounts");
        }
    }
}
