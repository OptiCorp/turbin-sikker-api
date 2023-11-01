using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class taskInfo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskInfo_Workflow_WorkflowId",
                table: "TaskInfo");

            migrationBuilder.AlterColumn<string>(
                name: "WorkflowId",
                table: "TaskInfo",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskInfo_Workflow_WorkflowId",
                table: "TaskInfo",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskInfo_Workflow_WorkflowId",
                table: "TaskInfo");

            migrationBuilder.AlterColumn<string>(
                name: "WorkflowId",
                table: "TaskInfo",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskInfo_Workflow_WorkflowId",
                table: "TaskInfo",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id");
        }
    }
}
