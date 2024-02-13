using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class accountsreak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ////migrationBuilder.DropForeignKey(
            ////    name: "FK_ContributionDetails_ContributionMasterDatas_ContributionMasterDataDataId",
            ////    table: "ContributionDetails");

            //migrationBuilder.DropTable(
            //    name: "ContributionMasterDatas");

            //migrationBuilder.DropIndex(
            //    name: "IX_ContributionDetails_ContributionMasterDataDataId",
            //    table: "ContributionDetails");

            //migrationBuilder.DropColumn(
            //    name: "ContributionMasterDataDataId",
            //    table: "ContributionDetails");

            //migrationBuilder.DropColumn(
            //    name: "ContributionMasterDataId",
            //    table: "ContributionDetails");

            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "AccountsDirectEntry");

            //migrationBuilder.AlterColumn<long>(
            //    name: "Id",
            //    table: "ContributionDetails",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<long>(
            //    name: "ContributionMasterId",
            //    table: "ContributionDetails",
            //    nullable: false,
            //    defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "AddedUser",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByByUserId",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedUser",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AccountsDirectEntry",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AccountsDirectEntry",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserId",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedUser",
                table: "AccountsDirectEntry",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MonthCode",
                table: "AccountsDirectEntry",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearOf",
                table: "AccountsDirectEntry",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Accounts",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "ContributionMasters",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedByUserId = table.Column<int>(nullable: true),
            //        AddedUser = table.Column<string>(nullable: true),
            //        CreatedDate = table.Column<DateTime>(nullable: true),
            //        ModifiedByUserId = table.Column<int>(nullable: true),
            //        ModifiedUser = table.Column<string>(nullable: true),
            //        ModifiedDate = table.Column<DateTime>(nullable: true),
            //        IsDeleted = table.Column<bool>(nullable: false),
            //        DeletedByByUserId = table.Column<int>(nullable: true),
            //        DeletedUser = table.Column<string>(nullable: true),
            //        DeletedDate = table.Column<DateTime>(nullable: true),
            //        FileName = table.Column<string>(nullable: true),
            //        FileLocation = table.Column<string>(nullable: true),
            //        FileType = table.Column<string>(nullable: true),
            //        FileExtension = table.Column<string>(nullable: true),
            //        FileSize = table.Column<decimal>(nullable: false),
            //        Month = table.Column<string>(nullable: true),
            //        Year = table.Column<string>(nullable: true),
            //        Circle = table.Column<string>(nullable: true),
            //        totalamount = table.Column<string>(nullable: true),
            //        totalentry = table.Column<string>(nullable: true),
            //        NewMemberCount = table.Column<string>(nullable: true),
            //        ContributionStatus = table.Column<string>(nullable: true),
            //        isApproved = table.Column<bool>(nullable: false),
            //        ApprovedBy = table.Column<string>(nullable: true),
            //        ApprovedDate = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ContributionMasters", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_ContributionMasters_User_CreatedByUserId",
            //            column: x => x.CreatedByUserId,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ContributionMasters_User_ModifiedByUserId",
            //            column: x => x.ModifiedByUserId,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContributionDetails_ContributionMasterId",
            //    table: "ContributionDetails",
            //    column: "ContributionMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsDirectEntry_CreatedByUserId",
                table: "AccountsDirectEntry",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsDirectEntry_ModifiedByUserId",
                table: "AccountsDirectEntry",
                column: "ModifiedByUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContributionMasters_CreatedByUserId",
            //    table: "ContributionMasters",
            //    column: "CreatedByUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContributionMasters_ModifiedByUserId",
            //    table: "ContributionMasters",
            //    column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsDirectEntry_User_CreatedByUserId",
                table: "AccountsDirectEntry",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsDirectEntry_User_ModifiedByUserId",
                table: "AccountsDirectEntry",
                column: "ModifiedByUserId",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId",
            //    table: "ContributionDetails",
            //    column: "ContributionMasterId",
            //    principalTable: "ContributionMasters",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsDirectEntry_User_CreatedByUserId",
                table: "AccountsDirectEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountsDirectEntry_User_ModifiedByUserId",
                table: "AccountsDirectEntry");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ContributionDetails_ContributionMasters_ContributionMasterId",
            //    table: "ContributionDetails");

            migrationBuilder.DropTable(
                name: "ContributionMasters");

            //migrationBuilder.DropIndex(
            //    name: "IX_ContributionDetails_ContributionMasterId",
            //    table: "ContributionDetails");

            migrationBuilder.DropIndex(
                name: "IX_AccountsDirectEntry_CreatedByUserId",
                table: "AccountsDirectEntry");

            migrationBuilder.DropIndex(
                name: "IX_AccountsDirectEntry_ModifiedByUserId",
                table: "AccountsDirectEntry");

            //migrationBuilder.DropColumn(
            //    name: "ContributionMasterId",
            //    table: "ContributionDetails");

            migrationBuilder.DropColumn(
                name: "AddedUser",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "DeletedByByUserId",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "DeletedUser",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "MonthCode",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "YearOf",
                table: "AccountsDirectEntry");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Accounts");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "ContributionDetails",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(long))
            //    .Annotation("SqlServer:Identity", "1, 1")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<int>(
            //    name: "ContributionMasterDataDataId",
            //    table: "ContributionDetails",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ContributionMasterDataId",
            //    table: "ContributionDetails",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "AccountsDirectEntry",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddedDate",
                table: "AccountsDirectEntry",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "ContributionMasterDatas",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AddedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ApprovedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Circle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContributionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FileLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FileSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        NewMemberCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        isApproved = table.Column<bool>(type: "bit", nullable: false),
            //        totalamount = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        totalentry = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ContributionMasterDatas", x => x.Id);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ContributionDetails_ContributionMasterDataDataId",
            //    table: "ContributionDetails",
            //    column: "ContributionMasterDataDataId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ContributionDetails_ContributionMasterDatas_ContributionMasterDataDataId",
            //    table: "ContributionDetails",
            //    column: "ContributionMasterDataDataId",
            //    principalTable: "ContributionMasterDatas",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}
