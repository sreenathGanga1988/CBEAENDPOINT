using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.EFCore.Migrations
{
    public partial class gettback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AccountConculsion",
            //    columns: table => new
            //    {
            //        CircleID = table.Column<int>(nullable: false),
            //        YearOF = table.Column<int>(nullable: false),
            //        MonthCode = table.Column<int>(nullable: false),
            //        NoOfRecords = table.Column<int>(nullable: false),
            //        NoOfMultipleRecords = table.Column<int>(nullable: true),
            //        NoOfDuplicatedRecords = table.Column<int>(nullable: true),
            //        NoOfActualRecords = table.Column<int>(nullable: true),
            //        Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
            //        CreditedAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AccountDuplication",
            //    columns: table => new
            //    {
            //        CircleID = table.Column<int>(nullable: false),
            //        YearOF = table.Column<int>(nullable: false),
            //        MonthCode = table.Column<int>(nullable: false),
            //        StaffNo = table.Column<int>(nullable: false),
            //        Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
            //        DuplicatedCircleID = table.Column<int>(nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AccountMultiEntry",
            //    columns: table => new
            //    {
            //        CircleID = table.Column<int>(nullable: false),
            //        YearOF = table.Column<int>(nullable: false),
            //        MonthCode = table.Column<int>(nullable: false),
            //        StaffNo = table.Column<int>(nullable: false),
            //        Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AccountsDirectEntry",
            //    columns: table => new
            //    {
            //        StaffNo = table.Column<double>(nullable: true),
            //        Name = table.Column<string>(maxLength: 255, nullable: true),
            //        DpCode = table.Column<double>(nullable: true),
            //        DD_IBA = table.Column<string>(name: "DD_IBA ", maxLength: 255, nullable: true),
            //        DD_IBADate = table.Column<string>(name: "DD_IBA Date", maxLength: 255, nullable: true),
            //        Amt = table.Column<double>(nullable: true),
            //        Enrl = table.Column<string>(maxLength: 255, nullable: true),
            //        Fine = table.Column<string>(maxLength: 255, nullable: true),
            //        F9 = table.Column<string>(maxLength: 255, nullable: true),
            //        F10 = table.Column<string>(maxLength: 255, nullable: true),
            //        F11 = table.Column<string>(maxLength: 255, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Month",
            //    columns: table => new
            //    {
            //        MonthCode = table.Column<int>(nullable: false),
            //        MonthName = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
            //        Abbrivation = table.Column<string>(unicode: false, maxLength: 3, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "State",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
            //        Abbreviation = table.Column<string>(unicode: false, maxLength: 7, nullable: false),
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_State", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "User",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserName = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
            //        Password = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
            //        UserTypeID = table.Column<int>(nullable: true),
            //        EmployeeID = table.Column<int>(nullable: true),
            //        IsActive = table.Column<bool>(nullable: false),
            //        ValidTill = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreatedByUserID = table.Column<int>(nullable: true),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        ModifiedByUserID = table.Column<int>(nullable: true),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_User", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserType",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserType = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
            //        Description = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserType", x => x.ID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Category",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
            //        Abbreviation = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Category", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Category_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Category_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Circle",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CircleCode = table.Column<int>(nullable: true),
            //        Name = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
            //        Abbreviation = table.Column<string>(unicode: false, maxLength: 7, nullable: false),
            //        IsActive = table.Column<bool>(nullable: false),
            //        StateID = table.Column<int>(nullable: true),
            //        DateFrom = table.Column<DateTime>(type: "datetime", nullable: true),
            //        DateTo = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Circle", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Circle_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Circle_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Circle_State",
            //            column: x => x.StateID,
            //            principalTable: "State",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Designation",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
            //        Description = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Designation", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Designation_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Designation_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Image",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Image = table.Column<byte[]>(nullable: false),
            //        Description = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByuserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Image", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Image_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Image_User1",
            //            column: x => x.ModifiedByuserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Settings",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CultureObject = table.Column<byte[]>(nullable: true),
            //        UserID = table.Column<int>(nullable: true),
            //        CurrencyName = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
            //        ApplicationClock = table.Column<int>(nullable: true),
            //        LineNumberFormat = table.Column<int>(nullable: true),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Settings", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Settings_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Settings_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Settings_User2",
            //            column: x => x.UserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Status",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
            //        Abbreviation = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
            //        Description = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
            //        GroupID = table.Column<int>(nullable: true),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Status", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Status_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Status_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserPreferences",
            //    columns: table => new
            //    {
            //        UserID = table.Column<int>(nullable: false),
            //        FormName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
            //        SettingName = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
            //        SettingValue = table.Column<string>(unicode: false, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.ForeignKey(
            //            name: "FK_UserPreferences_User",
            //            column: x => x.UserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Branch",
            //    columns: table => new
            //    {
            //        DpCode = table.Column<int>(nullable: false),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CircleID = table.Column<int>(nullable: false),
            //        StateID = table.Column<int>(nullable: true),
            //        Name = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
            //        Address1 = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
            //        Address2 = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
            //        Address3 = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
            //        District = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
            //        IsActive = table.Column<bool>(nullable: false),
            //        IsRegCompleted = table.Column<bool>(nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Branch", x => x.DpCode);
            //        table.ForeignKey(
            //            name: "FK_Branch_Circle",
            //            column: x => x.CircleID,
            //            principalTable: "Circle",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Branch_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Branch_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Branch_State",
            //            column: x => x.StateID,
            //            principalTable: "State",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CheckOffArrear",
            //    columns: table => new
            //    {
            //        CircleID = table.Column<int>(nullable: false),
            //        YearOF = table.Column<int>(nullable: false),
            //        MonthCode = table.Column<int>(nullable: false),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Reason = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CheckOffArrear_1", x => new { x.CircleID, x.YearOF, x.MonthCode });
            //        table.ForeignKey(
            //            name: "FK_CheckOffArrear_Circle",
            //            column: x => x.CircleID,
            //            principalTable: "Circle",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_CheckOffArrear_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_CheckOffArrear_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CircleState",
            //    columns: table => new
            //    {
            //        CircleID = table.Column<int>(nullable: false),
            //        StateID = table.Column<int>(nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.ForeignKey(
            //            name: "FK_CircleState_Circle",
            //            column: x => x.CircleID,
            //            principalTable: "Circle",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_CircleState_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_CircleState_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_CircleState_State",
            //            column: x => x.StateID,
            //            principalTable: "State",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Member",
            //    columns: table => new
            //    {
            //        StaffNo = table.Column<int>(nullable: false),
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
            //        GenderID = table.Column<int>(nullable: false),
            //        ImageID = table.Column<int>(nullable: true),
            //        DOB = table.Column<DateTime>(type: "datetime", nullable: true),
            //        DesignationID = table.Column<int>(nullable: true),
            //        CategoryID = table.Column<int>(nullable: true),
            //        DOJ = table.Column<DateTime>(type: "datetime", nullable: true),
            //        DpCode = table.Column<int>(nullable: true),
            //        DOJToScheme = table.Column<DateTime>(type: "datetime", nullable: true),
            //        StatusID = table.Column<int>(nullable: false),
            //        IsRegCompleted = table.Column<bool>(nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Member", x => x.StaffNo);
            //        table.ForeignKey(
            //            name: "FK_Member_Category",
            //            column: x => x.CategoryID,
            //            principalTable: "Category",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Member_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Member_Designation",
            //            column: x => x.DesignationID,
            //            principalTable: "Designation",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Member_Branch",
            //            column: x => x.DpCode,
            //            principalTable: "Branch",
            //            principalColumn: "DpCode",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Member_Image",
            //            column: x => x.ImageID,
            //            principalTable: "Image",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Member_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Member_Status",
            //            column: x => x.StatusID,
            //            principalTable: "Status",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Accounts",
            //    columns: table => new
            //    {
            //        ID = table.Column<long>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CircleID = table.Column<int>(nullable: false),
            //        DpCode = table.Column<int>(nullable: true),
            //        StaffNo = table.Column<int>(nullable: false),
            //        MonthCode = table.Column<int>(nullable: false),
            //        YearOF = table.Column<int>(nullable: false),
            //        Amount = table.Column<decimal>(type: "decimal(18, 3)", nullable: false),
            //        TransMode = table.Column<int>(nullable: false),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Accounts", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Accounts_Circle",
            //            column: x => x.CircleID,
            //            principalTable: "Circle",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Accounts_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Accounts_Branch",
            //            column: x => x.DpCode,
            //            principalTable: "Branch",
            //            principalColumn: "DpCode",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Accounts_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Accounts_Member",
            //            column: x => x.StaffNo,
            //            principalTable: "Member",
            //            principalColumn: "StaffNo",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Accounts_Status",
            //            column: x => x.TransMode,
            //            principalTable: "Status",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Transfer",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        StaffNo = table.Column<int>(nullable: false),
            //        DpCode = table.Column<int>(nullable: false),
            //        TransDate = table.Column<DateTime>(type: "datetime", nullable: true),
            //        CreatedByUserID = table.Column<int>(nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
            //        ModifiedByUserID = table.Column<int>(nullable: false),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Transfer", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_Transfer_User",
            //            column: x => x.CreatedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Transfer_Branch",
            //            column: x => x.DpCode,
            //            principalTable: "Branch",
            //            principalColumn: "DpCode",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Transfer_User1",
            //            column: x => x.ModifiedByUserID,
            //            principalTable: "User",
            //            principalColumn: "ID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Transfer_Member",
            //            column: x => x.StaffNo,
            //            principalTable: "Member",
            //            principalColumn: "StaffNo",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Accounts_CircleID",
            //    table: "Accounts",
            //    column: "CircleID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Accounts_CreatedByUserID",
            //    table: "Accounts",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Accounts_DpCode",
            //    table: "Accounts",
            //    column: "DpCode");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Accounts_ModifiedByUserID",
            //    table: "Accounts",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Accounts_StaffNo",
            //    table: "Accounts",
            //    column: "StaffNo");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Accounts_TransMode",
            //    table: "Accounts",
            //    column: "TransMode");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Branch_CircleID",
            //    table: "Branch",
            //    column: "CircleID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Branch_CreatedByUserID",
            //    table: "Branch",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Branch_ModifiedByUserID",
            //    table: "Branch",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Branch_StateID",
            //    table: "Branch",
            //    column: "StateID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Category_CreatedByUserID",
            //    table: "Category",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Category_ModifiedByUserID",
            //    table: "Category",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CheckOffArrear_CreatedByUserID",
            //    table: "CheckOffArrear",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CheckOffArrear_ModifiedByUserID",
            //    table: "CheckOffArrear",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Circle_CreatedByUserID",
            //    table: "Circle",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Circle_ModifiedByUserID",
            //    table: "Circle",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Circle_StateID",
            //    table: "Circle",
            //    column: "StateID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CircleState_CircleID",
            //    table: "CircleState",
            //    column: "CircleID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CircleState_CreatedByUserID",
            //    table: "CircleState",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CircleState_ModifiedByUserID",
            //    table: "CircleState",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_CircleState_StateID",
            //    table: "CircleState",
            //    column: "StateID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Designation_CreatedByUserID",
            //    table: "Designation",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Designation_ModifiedByUserID",
            //    table: "Designation",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Image_CreatedByUserID",
            //    table: "Image",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Image_ModifiedByuserID",
            //    table: "Image",
            //    column: "ModifiedByuserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Member_CategoryID",
            //    table: "Member",
            //    column: "CategoryID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Member_CreatedByUserID",
            //    table: "Member",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Member_DesignationID",
            //    table: "Member",
            //    column: "DesignationID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Member_DpCode",
            //    table: "Member",
            //    column: "DpCode");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Member_ImageID",
            //    table: "Member",
            //    column: "ImageID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Member_ModifiedByUserID",
            //    table: "Member",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Member_StatusID",
            //    table: "Member",
            //    column: "StatusID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Settings_CreatedByUserID",
            //    table: "Settings",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Settings_ModifiedByUserID",
            //    table: "Settings",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Settings_UserID",
            //    table: "Settings",
            //    column: "UserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Status_CreatedByUserID",
            //    table: "Status",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Status_ModifiedByUserID",
            //    table: "Status",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transfer_CreatedByUserID",
            //    table: "Transfer",
            //    column: "CreatedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transfer_DpCode",
            //    table: "Transfer",
            //    column: "DpCode");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transfer_ModifiedByUserID",
            //    table: "Transfer",
            //    column: "ModifiedByUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transfer_StaffNo",
            //    table: "Transfer",
            //    column: "StaffNo");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserPreferences_UserID",
            //    table: "UserPreferences",
            //    column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountConculsion");

            migrationBuilder.DropTable(
                name: "AccountDuplication");

            migrationBuilder.DropTable(
                name: "AccountMultiEntry");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountsDirectEntry");

            migrationBuilder.DropTable(
                name: "CheckOffArrear");

            migrationBuilder.DropTable(
                name: "CircleState");

            migrationBuilder.DropTable(
                name: "Month");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Transfer");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Designation");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Circle");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "State");
        }
    }
}
