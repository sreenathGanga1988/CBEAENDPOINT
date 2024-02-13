using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedconribuion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contribution",
                table: "ContributionDetails");

            migrationBuilder.AddColumn<int>(
                name: "ContributionMasterId",
                table: "ContributionDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ContributionMasterId1",
                table: "ContributionDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContributionDetails_ContributionMasterId1",
                table: "ContributionDetails",
                column: "ContributionMasterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId1",
                table: "ContributionDetails",
                column: "ContributionMasterId1",
                principalTable: "ContributionMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId1",
                table: "ContributionDetails");

            migrationBuilder.DropIndex(
                name: "IX_ContributionDetails_ContributionMasterId1",
                table: "ContributionDetails");

            migrationBuilder.DropColumn(
                name: "ContributionMasterId",
                table: "ContributionDetails");

            migrationBuilder.DropColumn(
                name: "ContributionMasterId1",
                table: "ContributionDetails");

            migrationBuilder.AddColumn<int>(
                name: "Contribution",
                table: "ContributionDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
