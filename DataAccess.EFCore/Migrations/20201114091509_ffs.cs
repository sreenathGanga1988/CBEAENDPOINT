using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class ffs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedDate",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "ContributionMasters",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "ContributionMasters");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "ContributionMasters");

            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "ContributionMasters");
        }
    }
}