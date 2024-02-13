using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedyearindeathclaimandparked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearOF",
                table: "DeathClaim",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Parkedon",
                table: "ContributionDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnParkedon",
                table: "ContributionDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearOF",
                table: "DeathClaim");

            migrationBuilder.DropColumn(
                name: "Parkedon",
                table: "ContributionDetails");

            migrationBuilder.DropColumn(
                name: "UnParkedon",
                table: "ContributionDetails");
        }
    }
}
