using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class updatechecklistworkflowusermodelupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_UserId",
                table: "ChecklistWorkflow",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistWorkflow_User_UserId",
                table: "ChecklistWorkflow",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistWorkflow_User_UserId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistWorkflow_UserId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChecklistWorkflow");
        }
    }
}
