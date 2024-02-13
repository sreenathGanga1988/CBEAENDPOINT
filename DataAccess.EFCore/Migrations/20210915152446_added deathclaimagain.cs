using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addeddeathclaimagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeathClaim",
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
                    StaffNo = table.Column<int>(nullable: false),
                    DpCode = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: true),
                    DeathDate = table.Column<DateTime>(nullable: true),
                    Nominee = table.Column<string>(nullable: true),
                    NomineeRelation = table.Column<string>(nullable: true),
                    NomineeIDentity = table.Column<string>(nullable: true),
                    DDNO = table.Column<string>(nullable: true),
                    DDDATE = table.Column<DateTime>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    LastContribution = table.Column<float>(nullable: false),
                    DpCodeNavigationDpCode = table.Column<int>(nullable: true),
                    StaffNoNavigationStaffNo = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeathClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeathClaim_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeathClaim_Branch_DpCodeNavigationDpCode",
                        column: x => x.DpCodeNavigationDpCode,
                        principalTable: "Branch",
                        principalColumn: "DpCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeathClaim_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeathClaim_Member_StaffNoNavigationStaffNo",
                        column: x => x.StaffNoNavigationStaffNo,
                        principalTable: "Member",
                        principalColumn: "StaffNo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeathClaim_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeathClaim_CreatedByUserId",
                table: "DeathClaim",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeathClaim_DpCodeNavigationDpCode",
                table: "DeathClaim",
                column: "DpCodeNavigationDpCode");

            migrationBuilder.CreateIndex(
                name: "IX_DeathClaim_ModifiedByUserId",
                table: "DeathClaim",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeathClaim_StaffNoNavigationStaffNo",
                table: "DeathClaim",
                column: "StaffNoNavigationStaffNo");

            migrationBuilder.CreateIndex(
                name: "IX_DeathClaim_StateId",
                table: "DeathClaim",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeathClaim");
        }
    }
}
