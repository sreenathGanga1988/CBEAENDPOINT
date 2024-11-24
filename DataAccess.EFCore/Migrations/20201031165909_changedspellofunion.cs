using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class changedspellofunion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnionMemeber",
                table: "Member");

            migrationBuilder.AddColumn<string>(
                name: "UnionMember",
                table: "Member",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnionMember",
                table: "Member");

            migrationBuilder.AddColumn<string>(
                name: "UnionMemeber",
                table: "Member",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}