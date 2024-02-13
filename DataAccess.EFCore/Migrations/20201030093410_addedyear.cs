using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedyear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "SupportTickets",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByUserId",
                table: "SupportTickets",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AddedUser",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByByUserId",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedUser",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SupportTickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SupportTickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                table: "SupportTickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nominee",
                table: "Member",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomineeIDentity",
                table: "Member",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomineeRelation",
                table: "Member",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnionMemeber",
                table: "Member",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClaimCounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedByUserId = table.Column<int>(nullable: true),
                    AddedUser = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    ModifiedByUserId = table.Column<int>(nullable: true),
                    ModifiedUser = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedByByUserId = table.Column<int>(nullable: true),
                    DeletedUser = table.Column<string>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    YearOF = table.Column<int>(nullable: false),
                    DesignationId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimCounts_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimCounts_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YearMasters",
                columns: table => new
                {
                    YearOf = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearMasters", x => x.YearOf);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_CreatedByUserId",
                table: "SupportTickets",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_ModifiedByUserId",
                table: "SupportTickets",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimCounts_CreatedByUserId",
                table: "ClaimCounts",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimCounts_ModifiedByUserId",
                table: "ClaimCounts",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_User_CreatedByUserId",
                table: "SupportTickets",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_User_ModifiedByUserId",
                table: "SupportTickets",
                column: "ModifiedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_User_CreatedByUserId",
                table: "SupportTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_User_ModifiedByUserId",
                table: "SupportTickets");

            migrationBuilder.DropTable(
                name: "ClaimCounts");

            migrationBuilder.DropTable(
                name: "YearMasters");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_CreatedByUserId",
                table: "SupportTickets");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_ModifiedByUserId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "AddedUser",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "DeletedByByUserId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "DeletedUser",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "Nominee",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "NomineeIDentity",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "NomineeRelation",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "UnionMemeber",
                table: "Member");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "SupportTickets",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedByUserId",
                table: "SupportTickets",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
