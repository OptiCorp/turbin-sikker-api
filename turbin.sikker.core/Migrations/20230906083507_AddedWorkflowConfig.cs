using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class AddedWorkflowConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChecklistId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_ChecklistId",
                table: "ChecklistWorkflow",
                column: "ChecklistId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistWorkflow_Checklist_ChecklistId",
                table: "ChecklistWorkflow",
                column: "ChecklistId",
                principalTable: "Checklist",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistWorkflow_Checklist_ChecklistId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistWorkflow_ChecklistId",
                table: "ChecklistWorkflow");

            migrationBuilder.AlterColumn<string>(
                name: "ChecklistId",
                table: "ChecklistWorkflow",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
