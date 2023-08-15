using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class refactoringChecklistWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistWorkflow_Checklist_WorkflowChecklistId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistWorkflow_User_RecipientId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistWorkflow_RecipientId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "ChecklistWorkflow");

            migrationBuilder.RenameColumn(
                name: "WorkflowChecklistId",
                table: "ChecklistWorkflow",
                newName: "ChecklistWorkflowId");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistWorkflow_WorkflowChecklistId",
                table: "ChecklistWorkflow",
                newName: "IX_ChecklistWorkflow_ChecklistWorkflowId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistWorkflow_Checklist_ChecklistWorkflowId",
                table: "ChecklistWorkflow",
                column: "ChecklistWorkflowId",
                principalTable: "Checklist",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistWorkflow_Checklist_ChecklistWorkflowId",
                table: "ChecklistWorkflow");

            migrationBuilder.RenameColumn(
                name: "ChecklistWorkflowId",
                table: "ChecklistWorkflow",
                newName: "WorkflowChecklistId");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistWorkflow_ChecklistWorkflowId",
                table: "ChecklistWorkflow",
                newName: "IX_ChecklistWorkflow_WorkflowChecklistId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_RecipientId",
                table: "ChecklistWorkflow",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistWorkflow_Checklist_WorkflowChecklistId",
                table: "ChecklistWorkflow",
                column: "WorkflowChecklistId",
                principalTable: "Checklist",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistWorkflow_User_RecipientId",
                table: "ChecklistWorkflow",
                column: "RecipientId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
