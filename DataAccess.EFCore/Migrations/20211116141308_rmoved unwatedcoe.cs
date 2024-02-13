using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class rmovedunwatedcoe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountConculsion");

            migrationBuilder.DropTable(
                name: "AccountDuplication");

            migrationBuilder.DropTable(
                name: "AccountMultiEntry");

            migrationBuilder.DropTable(
                name: "CheckOffArrear");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountConculsion",
                columns: table => new
                {
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CircleID = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreditedAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MonthCode = table.Column<int>(type: "int", nullable: false),
                    NoOfActualRecords = table.Column<int>(type: "int", nullable: true),
                    NoOfDuplicatedRecords = table.Column<int>(type: "int", nullable: true),
                    NoOfMultipleRecords = table.Column<int>(type: "int", nullable: true),
                    NoOfRecords = table.Column<int>(type: "int", nullable: false),
                    YearOF = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AccountDuplication",
                columns: table => new
                {
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CircleID = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DuplicatedCircleID = table.Column<int>(type: "int", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MonthCode = table.Column<int>(type: "int", nullable: false),
                    StaffNo = table.Column<int>(type: "int", nullable: false),
                    YearOF = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AccountMultiEntry",
                columns: table => new
                {
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CircleID = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MonthCode = table.Column<int>(type: "int", nullable: false),
                    StaffNo = table.Column<int>(type: "int", nullable: false),
                    YearOF = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CheckOffArrear",
                columns: table => new
                {
                    CircleID = table.Column<int>(type: "int", nullable: false),
                    YearOF = table.Column<int>(type: "int", nullable: false),
                    MonthCode = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedByUserID = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Reason = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOffArrear_1", x => new { x.CircleID, x.YearOF, x.MonthCode });
                    table.ForeignKey(
                        name: "FK_CheckOffArrear_Circle",
                        column: x => x.CircleID,
                        principalTable: "Circle",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckOffArrear_User",
                        column: x => x.CreatedByUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckOffArrear_User1",
                        column: x => x.ModifiedByUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckOffArrear_CreatedByUserID",
                table: "CheckOffArrear",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOffArrear_ModifiedByUserID",
                table: "CheckOffArrear",
                column: "ModifiedByUserID");
        }
    }
}
