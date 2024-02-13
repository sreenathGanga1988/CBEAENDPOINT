using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedrefundmaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefundContribution",
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
                    StateId = table.Column<int>(nullable: true),
                    DesignationId = table.Column<int>(nullable: true),
                    DeathDate = table.Column<DateTime>(nullable: true),
                    DDNO = table.Column<string>(nullable: true),
                    DDDATE = table.Column<DateTime>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    LastContribution = table.Column<float>(nullable: false),
                    YearOF = table.Column<int>(nullable: false),
                    StaffNoNavigationStaffNo = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundContribution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundContribution_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundContribution_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundContribution_User_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundContribution_Member_StaffNoNavigationStaffNo",
                        column: x => x.StaffNoNavigationStaffNo,
                        principalTable: "Member",
                        principalColumn: "StaffNo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundContribution_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefundContribution_CreatedByUserId",
                table: "RefundContribution",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundContribution_DesignationId",
                table: "RefundContribution",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundContribution_ModifiedByUserId",
                table: "RefundContribution",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundContribution_StaffNoNavigationStaffNo",
                table: "RefundContribution",
                column: "StaffNoNavigationStaffNo");

            migrationBuilder.CreateIndex(
                name: "IX_RefundContribution_StateId",
                table: "RefundContribution",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefundContribution");
        }
    }
}
