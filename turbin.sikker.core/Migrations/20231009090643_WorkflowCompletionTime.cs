using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class WorkflowCompletionTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursSpent",
                table: "Workflow");

            migrationBuilder.AddColumn<int>(
                name: "CompletionTimeMinutes",
                table: "Workflow",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionTimeMinutes",
                table: "Workflow");

            migrationBuilder.AddColumn<float>(
                name: "HoursSpent",
                table: "Workflow",
                type: "real",
                nullable: true);
        }
    }
}
