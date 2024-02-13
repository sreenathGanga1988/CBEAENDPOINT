using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedloginsupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoggedTime",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WrongTryCount",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastLoggedTime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WrongTryCount",
                table: "User");
        }
    }
}
