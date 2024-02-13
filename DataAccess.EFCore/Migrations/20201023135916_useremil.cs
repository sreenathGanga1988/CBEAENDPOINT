using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class useremil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AccountsDirectEntry",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "EmailId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNum",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNum",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "AccountsDirectEntry",
                newName: "Id");
        }
    }
}
