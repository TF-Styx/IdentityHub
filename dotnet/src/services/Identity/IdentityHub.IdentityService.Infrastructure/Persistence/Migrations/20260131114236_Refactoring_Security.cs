using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityHub.IdentityService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Refactoring_Security : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSalt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EncryptedDek",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HashPassword",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Mail",
                table: "Users",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "text",
                nullable: true,
                collation: "case_insensitive");

            migrationBuilder.CreateTable(
                name: "AuthMethod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthType = table.Column<int>(type: "integer", nullable: false),
                    AuthKey = table.Column<string>(type: "text", nullable: false),
                    AuthData = table.Column<string>(type: "text", nullable: true),
                    Salt = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthMethod_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecureData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecureDataType = table.Column<int>(type: "integer", nullable: false),
                    SecureEncryptedValue = table.Column<string>(type: "text", nullable: false),
                    SecureEncryptedMetadata = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecureData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecureData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthMethod_UserId",
                table: "AuthMethod",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecureData_UserId",
                table: "SecureData",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthMethod");

            migrationBuilder.DropTable(
                name: "SecureData");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Mail");

            migrationBuilder.AddColumn<string>(
                name: "ClientSalt",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedDek",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HashPassword",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
