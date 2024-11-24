using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedcontribution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContributionDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullString = table.Column<string>(nullable: true),
                    Circle = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    DpCode = table.Column<string>(nullable: true),
                    StaffNo = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    Contribution = table.Column<int>(nullable: false),
                    Total = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContributionMasters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    FileLocation = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    FileExtension = table.Column<string>(nullable: true),
                    FileSize = table.Column<decimal>(nullable: false),
                    Month = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    Circle = table.Column<string>(nullable: true),
                    totalamount = table.Column<string>(nullable: true),
                    totalentry = table.Column<string>(nullable: true),
                    NewMemberCount = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionMasters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContributionDetails");

            migrationBuilder.DropTable(
                name: "ContributionMasters");
        }
    }
}