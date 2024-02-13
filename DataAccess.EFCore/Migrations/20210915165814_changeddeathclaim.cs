using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class changeddeathclaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeathClaim_Branch_DpCodeNavigationDpCode",
                table: "DeathClaim");

            migrationBuilder.DropIndex(
                name: "IX_DeathClaim_DpCodeNavigationDpCode",
                table: "DeathClaim");

            migrationBuilder.DropColumn(
                name: "DpCode",
                table: "DeathClaim");

            migrationBuilder.DropColumn(
                name: "DpCodeNavigationDpCode",
                table: "DeathClaim");

            migrationBuilder.AddColumn<int>(
                name: "DesignationId",
                table: "DeathClaim",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeathClaim_DesignationId",
                table: "DeathClaim",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeathClaim_Designation_DesignationId",
                table: "DeathClaim",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeathClaim_Designation_DesignationId",
                table: "DeathClaim");

            migrationBuilder.DropIndex(
                name: "IX_DeathClaim_DesignationId",
                table: "DeathClaim");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "DeathClaim");

            migrationBuilder.AddColumn<int>(
                name: "DpCode",
                table: "DeathClaim",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DpCodeNavigationDpCode",
                table: "DeathClaim",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeathClaim_DpCodeNavigationDpCode",
                table: "DeathClaim",
                column: "DpCodeNavigationDpCode");

            migrationBuilder.AddForeignKey(
                name: "FK_DeathClaim_Branch_DpCodeNavigationDpCode",
                table: "DeathClaim",
                column: "DpCodeNavigationDpCode",
                principalTable: "Branch",
                principalColumn: "DpCode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
