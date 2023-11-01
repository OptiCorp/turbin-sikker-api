using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class compositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskInfo",
                table: "TaskInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskInfo",
                table: "TaskInfo",
                columns: new[] { "TaskId", "WorkflowId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskInfo",
                table: "TaskInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskInfo",
                table: "TaskInfo",
                column: "TaskId");
        }
    }
}
