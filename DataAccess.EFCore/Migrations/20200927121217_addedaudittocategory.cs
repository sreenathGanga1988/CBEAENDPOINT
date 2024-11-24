using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedaudittocategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedUser",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByByUserId",
                table: "Category",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DeletedUser",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleteddate",
                table: "Category",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Category",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Category",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                table: "Category",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedUser",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeletedByByUserId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeletedUser",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Deleteddate",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                table: "Category");
        }
    }
}