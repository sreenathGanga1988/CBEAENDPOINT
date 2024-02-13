using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedcontactdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId",
                table: "ContributionDetails");

            migrationBuilder.AddColumn<string>(
                name: "ContactDesc1",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactDesc2",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactLine1",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactLine2",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactLine3",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Faxnum",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phonenum",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "MainPages",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ContributionMasterId",
                table: "ContributionDetails",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddedDate",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedDate",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "AccountsDirectEntry",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId",
                table: "ContributionDetails",
                column: "ContributionMasterId",
                principalTable: "ContributionMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId",
                table: "ContributionDetails");

            migrationBuilder.DropColumn(
                name: "ContactDesc1",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "ContactDesc2",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "ContactLine1",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "ContactLine2",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "ContactLine3",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "Faxnum",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "Phonenum",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "MainPages");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "status",
                table: "AccountsDirectEntry");

            migrationBuilder.AlterColumn<long>(
                name: "ContributionMasterId",
                table: "ContributionDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId",
                table: "ContributionDetails",
                column: "ContributionMasterId",
                principalTable: "ContributionMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
