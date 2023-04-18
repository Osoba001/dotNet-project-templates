using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class renameRecoveryPinCreationTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecoveryPinCreatedTime",
                table: "UserTb",
                newName: "RecoveryPinCreationTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecoveryPinCreationTime",
                table: "UserTb",
                newName: "RecoveryPinCreatedTime");
        }
    }
}
