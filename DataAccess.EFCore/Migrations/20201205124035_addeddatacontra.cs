using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addeddatacontra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContributionDetails");

            migrationBuilder.DropTable(
                name: "ContributionMasterDatas");

            migrationBuilder.CreateTable(
                name: "ContributionMasterDatasData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
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
                    isApproved = table.Column<bool>(nullable: false),
                    ContributionStatus = table.Column<string>(nullable: true),
                    AddedBy = table.Column<string>(nullable: true),
                    AddedDate = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionMasterDatasData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContributionDetailsData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullString = table.Column<string>(nullable: true),
                    Circle = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    DpCode = table.Column<string>(nullable: true),
                    StaffNo = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    ContributionMasterDataId = table.Column<int>(nullable: false),
                    Total = table.Column<string>(nullable: true),
                    ContributionMasterDataDataId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionDetailsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContributionDetailsData_ContributionMasterDatasData_ContributionMasterDataDataId",
                        column: x => x.ContributionMasterDataDataId,
                        principalTable: "ContributionMasterDatasData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContributionDetailsData_ContributionMasterDataDataId",
                table: "ContributionDetailsData",
                column: "ContributionMasterDataDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.DropTable(
            //        name: "ContributionDetailsData");

            //    migrationBuilder.DropTable(
            //        name: "ContributionMasterDatasData");

            //    migrationBuilder.CreateTable(
            //        name: "ContributionMasterDatas",
            //        columns: table => new
            //        {
            //            Id = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            AddedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ApprovedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Circle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ContributionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            FileSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //            FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            NewMemberCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            isApproved = table.Column<bool>(type: "bit", nullable: false),
            //            totalamount = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            totalentry = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_ContributionMasterDatas", x => x.Id);
            //        });

            //    migrationBuilder.CreateTable(
            //        name: "ContributionDetails",
            //        columns: table => new
            //        {
            //            Id = table.Column<long>(type: "bigint", nullable: false)
            //                .Annotation("SqlServer:Identity", "1, 1"),
            //            Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //            Circle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            ContributionMasterDataId = table.Column<long>(type: "bigint", nullable: false),
            //            Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            DpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            FullString = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            StaffNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Total = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //            Year = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_ContributionDetails", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_ContributionDetails_ContributionMasterDatas_ContributionMasterDataId",
            //                column: x => x.ContributionMasterDataId,
            //                principalTable: "ContributionMasterDatas",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Cascade);
            //        });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ContributionDetails_ContributionMasterDataId",
            //        table: "ContributionDetails",
            //        column: "ContributionMasterDataId");
        }
    }
}