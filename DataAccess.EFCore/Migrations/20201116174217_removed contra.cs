using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class removedcontra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId1",
                table: "ContributionDetails");

            migrationBuilder.DropIndex(
                name: "IX_ContributionDetails_ContributionMasterId1",
                table: "ContributionDetails");

            migrationBuilder.DropColumn(
                name: "ContributionMasterId1",
                table: "ContributionDetails");

            migrationBuilder.AlterColumn<long>(
                name: "ContributionMasterId",
                table: "ContributionDetails",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionDetails_ContributionMasterId",
                table: "ContributionDetails",
                column: "ContributionMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId",
                table: "ContributionDetails",
                column: "ContributionMasterId",
                principalTable: "ContributionMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId",
                table: "ContributionDetails");

            migrationBuilder.DropIndex(
                name: "IX_ContributionDetails_ContributionMasterId",
                table: "ContributionDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ContributionMasterId",
                table: "ContributionDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ContributionMasterId1",
                table: "ContributionDetails",
                type: "bigint",
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
    }
}
