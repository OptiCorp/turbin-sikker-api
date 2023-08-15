using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class refactoringChecklistWorkflow3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistWorkflow_User_UserId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistWorkflow_ChecklistWorkflowId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistWorkflow_UserId",
                table: "ChecklistWorkflow");



            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ChecklistWorkflow",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ChecklistWorkflowId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_ChecklistWorkflowId",
                table: "ChecklistWorkflow",
                column: "ChecklistWorkflowId");

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
    }
}
