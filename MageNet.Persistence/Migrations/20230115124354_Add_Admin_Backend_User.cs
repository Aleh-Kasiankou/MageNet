using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MageNet.Persistence.Migrations
{
    public partial class Add_Admin_Backend_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BackendUsers",
                columns: new[] { "BackendUserId", "PasswordHash", "UserName" },
                values: new object[] { new Guid("94e85a54-c2a9-4dcb-b123-6829be4f9d2c"), "K7eZhJaJms3YE3+tOkT6+WqEoD1/IwzkLpfNF8euQp4=", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BackendUsers",
                keyColumn: "BackendUserId",
                keyValue: new Guid("94e85a54-c2a9-4dcb-b123-6829be4f9d2c"));
        }
    }
}
