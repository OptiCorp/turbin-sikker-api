using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class checklistworklflow_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChecklistWorkflowId",
                table: "ChecklistWorkflow",
                newName: "ChecklistIdentity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChecklistIdentity",
                table: "ChecklistWorkflow",
                newName: "ChecklistWorkflowId");
        }
    }
}
