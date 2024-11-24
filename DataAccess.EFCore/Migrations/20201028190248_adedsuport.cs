using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccess.EFCore.Migrations
{
    public partial class adedsuport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupportTicketNum = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Priority = table.Column<string>(nullable: true),
                    CreatedByUserId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<string>(nullable: true),
                    DeveloperRemark = table.Column<string>(nullable: true),
                    isApproved = table.Column<bool>(nullable: false),
                    ApprovedByUserId = table.Column<int>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportTickets");
        }
    }
}