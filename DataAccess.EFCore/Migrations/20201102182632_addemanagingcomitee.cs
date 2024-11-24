using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.EFCore.Migrations
{
    public partial class addemanagingcomitee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManagingComitees",
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
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Description1 = table.Column<string>(nullable: true),
                    Description2 = table.Column<string>(nullable: true),
                    imageLocation = table.Column<string>(nullable: true),
                    order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagingComitees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagingComitees_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ManagingComitees_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagingComitees_CreatedByUserId",
                table: "ManagingComitees",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagingComitees_ModifiedByUserId",
                table: "ManagingComitees",
                column: "ModifiedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagingComitees");
        }
    }
}