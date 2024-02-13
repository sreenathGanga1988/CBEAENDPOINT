using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.EFCore.Migrations
{
    public partial class addenwwitm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedUser",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByByUserId",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedUser",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ContributionMasters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContributionMasters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ContributionMasters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                table: "ContributionMasters",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_ContributionMasters_CreatedByUserId",
                table: "ContributionMasters",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionMasters_ModifiedByUserId",
                table: "ContributionMasters",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
            name: "FK_ContributionMasters_User_CreatedByUserId",
            table: "ContributionMasters",
            column: "CreatedByUserId",
            principalTable: "User",
            principalColumn: "ID",
            onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContributionMasters_User_ModifiedByUserId",
                table: "ContributionMasters",
                column: "ModifiedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
