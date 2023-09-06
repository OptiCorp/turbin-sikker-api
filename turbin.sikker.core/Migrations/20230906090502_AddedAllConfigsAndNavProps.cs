using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class AddedAllConfigsAndNavProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PunchId",
                table: "Upload",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ChecklistWorkflowId",
                table: "Punch",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ChecklistTaskId",
                table: "Punch",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "ChecklistWorkflow",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Upload_PunchId",
                table: "Upload",
                column: "PunchId");

            migrationBuilder.CreateIndex(
                name: "IX_Punch_ChecklistTaskId",
                table: "Punch",
                column: "ChecklistTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Punch_ChecklistWorkflowId",
                table: "Punch",
                column: "ChecklistWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_CreatedById",
                table: "ChecklistWorkflow",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_UserId",
                table: "ChecklistWorkflow",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistWorkflow_User_CreatedById",
                table: "ChecklistWorkflow",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistWorkflow_User_UserId",
                table: "ChecklistWorkflow",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Punch_ChecklistWorkflow_ChecklistWorkflowId",
                table: "Punch",
                column: "ChecklistWorkflowId",
                principalTable: "ChecklistWorkflow",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Punch_Checklist_Task_ChecklistTaskId",
                table: "Punch",
                column: "ChecklistTaskId",
                principalTable: "Checklist_Task",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Upload_Punch_PunchId",
                table: "Upload",
                column: "PunchId",
                principalTable: "Punch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistWorkflow_User_CreatedById",
                table: "ChecklistWorkflow");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistWorkflow_User_UserId",
                table: "ChecklistWorkflow");

            migrationBuilder.DropForeignKey(
                name: "FK_Punch_ChecklistWorkflow_ChecklistWorkflowId",
                table: "Punch");

            migrationBuilder.DropForeignKey(
                name: "FK_Punch_Checklist_Task_ChecklistTaskId",
                table: "Punch");

            migrationBuilder.DropForeignKey(
                name: "FK_Upload_Punch_PunchId",
                table: "Upload");

            migrationBuilder.DropIndex(
                name: "IX_Upload_PunchId",
                table: "Upload");

            migrationBuilder.DropIndex(
                name: "IX_Punch_ChecklistTaskId",
                table: "Punch");

            migrationBuilder.DropIndex(
                name: "IX_Punch_ChecklistWorkflowId",
                table: "Punch");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistWorkflow_CreatedById",
                table: "ChecklistWorkflow");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistWorkflow_UserId",
                table: "ChecklistWorkflow");

            migrationBuilder.AlterColumn<string>(
                name: "PunchId",
                table: "Upload",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ChecklistWorkflowId",
                table: "Punch",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ChecklistTaskId",
                table: "Punch",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ChecklistWorkflow",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "ChecklistWorkflow",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
