using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addednews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainPages",
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
                    CorouselImage1 = table.Column<string>(nullable: true),
                    CorouselImage2 = table.Column<string>(nullable: true),
                    CorouselImage3 = table.Column<string>(nullable: true),
                    MainText = table.Column<string>(nullable: true),
                    Slogan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainPages_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MainPages_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateofAction = table.Column<DateTime>(nullable: false),
                    NewsText = table.Column<string>(nullable: true),
                    NewsLink = table.Column<string>(nullable: true),
                    MainPageId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsItems_MainPages_MainPageId",
                        column: x => x.MainPageId,
                        principalTable: "MainPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainPages_CreatedByUserId",
                table: "MainPages",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MainPages_ModifiedByUserId",
                table: "MainPages",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_MainPageId",
                table: "NewsItems",
                column: "MainPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsItems");

            migrationBuilder.DropTable(
                name: "MainPages");
        }
    }
}
