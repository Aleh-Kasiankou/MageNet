using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MageNet.Persistence.Migrations
{
    public partial class Add_Backend_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackendUsers",
                columns: table => new
                {
                    BackendUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendUsers", x => x.BackendUserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackendUsers_UserName",
                table: "BackendUsers",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackendUsers");
        }
    }
}
