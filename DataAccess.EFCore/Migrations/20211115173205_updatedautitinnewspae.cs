using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class updatedautitinnewspae : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayQuotes_User_CreatedByUserId",
                table: "DayQuotes");

            migrationBuilder.DropForeignKey(
                name: "FK_DayQuotes_User_ModifiedByUserId",
                table: "DayQuotes");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedByUserId",
                table: "DayQuotes",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByUserId",
                table: "DayQuotes",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AddedUser",
                table: "DayQuotes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByByUserId",
                table: "DayQuotes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "DayQuotes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedUser",
                table: "DayQuotes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DayQuotes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DayQuotes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                table: "DayQuotes",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DayQuotes_User_CreatedByUserId",
                table: "DayQuotes",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DayQuotes_User_ModifiedByUserId",
                table: "DayQuotes",
                column: "ModifiedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayQuotes_User_CreatedByUserId",
                table: "DayQuotes");

            migrationBuilder.DropForeignKey(
                name: "FK_DayQuotes_User_ModifiedByUserId",
                table: "DayQuotes");

            migrationBuilder.DropColumn(
                name: "AddedUser",
                table: "DayQuotes");

            migrationBuilder.DropColumn(
                name: "DeletedByByUserId",
                table: "DayQuotes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "DayQuotes");

            migrationBuilder.DropColumn(
                name: "DeletedUser",
                table: "DayQuotes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DayQuotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DayQuotes");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                table: "DayQuotes");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedByUserId",
                table: "DayQuotes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByUserId",
                table: "DayQuotes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DayQuotes_User_CreatedByUserId",
                table: "DayQuotes",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DayQuotes_User_ModifiedByUserId",
                table: "DayQuotes",
                column: "ModifiedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
