using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class rmovedtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "UserPreferences");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationClock = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CultureObject = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CurrencyName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LineNumberFormat = table.Column<int>(type: "int", nullable: true),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Settings_User",
                        column: x => x.CreatedByUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Settings_User1",
                        column: x => x.ModifiedByUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Settings_User2",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    FormName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    SettingName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    SettingValue = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_UserPreferences_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_CreatedByUserID",
                table: "Settings",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_ModifiedByUserID",
                table: "Settings",
                column: "ModifiedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_UserID",
                table: "Settings",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserID",
                table: "UserPreferences",
                column: "UserID");
        }
    }
}
