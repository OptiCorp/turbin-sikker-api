using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class TaskCompletionTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstAvgCompletionTime",
                table: "Checklist_Task",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstAvgCompletionTime",
                table: "Checklist_Task");
        }
    }
}
