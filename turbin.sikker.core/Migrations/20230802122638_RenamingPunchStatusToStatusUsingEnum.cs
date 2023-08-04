using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class RenamingPunchStatusToStatusUsingEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PunchStatus",
                table: "Punch",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Punch",
                newName: "PunchStatus");
        }
    }
}
