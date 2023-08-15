using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class refactoringChecklistWorkflow2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ChecklistWorkflow_Checklist_ChecklistId",
            //    table: "ChecklistWorkflow");

            //migrationBuilder.DropIndex(
            //    name: "IX_ChecklistWorkflow_ChecklistId",
            //    table: "ChecklistWorkflow");

            //migrationBuilder.DropColumn(
            //    name: "ChecklistId",
            //    table: "ChecklistWorkflow");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChecklistId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: true);

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
    }
}
