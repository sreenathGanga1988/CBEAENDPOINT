using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DayQuote",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RulesRegulation",
                table: "MainPages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayQuote",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "RulesRegulation",
                table: "MainPages");
        }
    }
}
