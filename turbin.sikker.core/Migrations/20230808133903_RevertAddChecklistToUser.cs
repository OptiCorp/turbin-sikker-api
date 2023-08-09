using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class RevertAddChecklistToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_User_UserId",
                table: "Checklist");

            migrationBuilder.DropIndex(
                name: "IX_Checklist_UserId",
                table: "Checklist");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Checklist");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Checklist",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_UserId",
                table: "Checklist",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_User_UserId",
                table: "Checklist",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
