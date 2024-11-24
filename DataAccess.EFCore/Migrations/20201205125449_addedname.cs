using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class addedname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContributionDetailsData_ContributionMasterDatasData_ContributionMasterDataDataId",
                table: "ContributionDetailsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContributionMasterDatasData",
                table: "ContributionMasterDatasData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContributionDetailsData",
                table: "ContributionDetailsData");

            migrationBuilder.RenameTable(
                name: "ContributionMasterDatasData",
                newName: "ContributionMasterDatas");

            migrationBuilder.RenameTable(
                name: "ContributionDetailsData",
                newName: "ContributionDetails");

            migrationBuilder.RenameIndex(
                name: "IX_ContributionDetailsData_ContributionMasterDataDataId",
                table: "ContributionDetails",
                newName: "IX_ContributionDetails_ContributionMasterDataDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContributionMasterDatas",
                table: "ContributionMasterDatas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContributionDetails",
                table: "ContributionDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContributionDetails_ContributionMasterDatas_ContributionMasterDataDataId",
                table: "ContributionDetails",
                column: "ContributionMasterDataDataId",
                principalTable: "ContributionMasterDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ContributionDetails_ContributionMasterDatas_ContributionMasterDataDataId",
            //    table: "ContributionDetails");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_ContributionMasterDatas",
            //    table: "ContributionMasterDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContributionDetails",
                table: "ContributionDetails");

            migrationBuilder.RenameTable(
                name: "ContributionMasterDatas",
                newName: "ContributionMasterDatasData");

            migrationBuilder.RenameTable(
                name: "ContributionDetails",
                newName: "ContributionDetailsData");

            migrationBuilder.RenameIndex(
                name: "IX_ContributionDetails_ContributionMasterDataDataId",
                table: "ContributionDetailsData",
                newName: "IX_ContributionDetailsData_ContributionMasterDataDataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContributionMasterDatasData",
                table: "ContributionMasterDatasData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContributionDetailsData",
                table: "ContributionDetailsData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContributionDetailsData_ContributionMasterDatasData_ContributionMasterDataDataId",
                table: "ContributionDetailsData",
                column: "ContributionMasterDataDataId",
                principalTable: "ContributionMasterDatasData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}