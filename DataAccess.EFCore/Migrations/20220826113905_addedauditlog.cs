using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedauditlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogs",
                table: "UserLogs");

            migrationBuilder.RenameTable(
                name: "UserLogs",
                newName: "UserLog");

          
           
            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLog",
                table: "UserLog",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "App_EntityLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(nullable: true),
                    EntityPrimaryKey = table.Column<string>(nullable: true),
                    ActionType = table.Column<string>(nullable: true),
                    DisplayAction = table.Column<string>(nullable: true),
                    NewContent = table.Column<string>(nullable: true),
                    ActionTime = table.Column<DateTime>(nullable: true),
                    CreatedByUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App_EntityLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "App_EntityLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLog",
                table: "UserLog");

           

            migrationBuilder.RenameTable(
                name: "UserLog",
                newName: "UserLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogs",
                table: "UserLogs",
                column: "Id");
        }
    }
}
