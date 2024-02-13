using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedkeytodirectentrynew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AccountsDirectEntry",
                newName: "ID");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "AccountsDirectEntry",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountsDirectEntry",
                table: "AccountsDirectEntry",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountsDirectEntry",
                table: "AccountsDirectEntry");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "AccountsDirectEntry",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AccountsDirectEntry",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
