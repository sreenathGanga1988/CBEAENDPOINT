using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedaudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedUser",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByByUserId",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedUser",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "NewsItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "NewsItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                table: "NewsItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_CreatedByUserId",
                table: "NewsItems",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_ModifiedByUserId",
                table: "NewsItems",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsItems_User_CreatedByUserId",
                table: "NewsItems",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsItems_User_ModifiedByUserId",
                table: "NewsItems",
                column: "ModifiedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsItems_User_CreatedByUserId",
                table: "NewsItems");

            migrationBuilder.DropForeignKey(
                name: "FK_NewsItems_User_ModifiedByUserId",
                table: "NewsItems");

            migrationBuilder.DropIndex(
                name: "IX_NewsItems_CreatedByUserId",
                table: "NewsItems");

            migrationBuilder.DropIndex(
                name: "IX_NewsItems_ModifiedByUserId",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "AddedUser",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "DeletedByByUserId",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "DeletedUser",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                table: "NewsItems");
        }
    }
}
