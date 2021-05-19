using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class ServerCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.UserGroupId);
                });

            migrationBuilder.CreateTable(
                name: "UserState",
                columns: table => new
                {
                    UserStateId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserState", x => x.UserStateId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    UserStateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK__User__UserGroupI__2C3393D0",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroup",
                        principalColumn: "UserGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__User__UserStateI__2D27B809",
                        column: x => x.UserStateId,
                        principalTable: "UserState",
                        principalColumn: "UserStateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_UserGroupId",
                table: "User",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserStateId",
                table: "User",
                column: "UserStateId");

            migrationBuilder.CreateIndex(
                name: "UQ__User__5E55825BB7FEDABC",
                table: "User",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__UserGrou__A25C5AA7D3528F23",
                table: "UserGroup",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__UserStat__A25C5AA7E06D2D0B",
                table: "UserState",
                column: "Code",
                unique: true);

            var columns = new[] { "UserGroupId", "Code", "Description" };
            migrationBuilder.InsertData(
                table: "UserGroup",
                columns: columns,
                values: new object[] { 1, "Admin", "Administartor" });
            migrationBuilder.InsertData(
                table: "UserGroup",
                columns: columns,
                values: new object[] { 2, "User", "Regular user" });

            columns = new[] { "UserStateId", "Code", "Description" };
            migrationBuilder.InsertData(
                table: "UserState",
                columns: columns,
                values: new object[] { 1, "Active", "Registered user" });
            migrationBuilder.InsertData(
                table: "UserState",
                columns: columns,
                values: new object[] { 2, "Blocked", "Deleted user" });

            columns = new[] { "Login", "Password", "UserGroupId", "UserStateId" };
            migrationBuilder.InsertData(
                table: "User",
                columns: columns,
                values: new object[] { "dmitry", "12345678", 1, 1 });
            migrationBuilder.InsertData(
                table: "User",
                columns: columns,
                values: new object[] { "sergey", "12345678", 2, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserState");
        }
    }
}
