using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTb",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFalseDeleted = table.Column<bool>(type: "bit", nullable: false),
                    FalseDeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllowSetNewPassword = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshTokenExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PasswordRecoveryPin = table.Column<int>(type: "int", nullable: false),
                    RecoveryPinCreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTb", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTb_Email",
                table: "UserTb",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTb");
        }
    }
}
