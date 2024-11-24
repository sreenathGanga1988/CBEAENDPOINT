using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedlogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoImage1",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoImage2",
                table: "MainPages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoImage1",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "LogoImage2",
                table: "MainPages");
        }
    }
}