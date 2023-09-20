using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class workflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_User_CreatedBy",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_Punch_ChecklistWorkflow_ChecklistWorkflowId",
                table: "Punch");

            migrationBuilder.DropForeignKey(
                name: "FK_Punch_User_CreatedBy",
                table: "Punch");

            migrationBuilder.DropTable(
                name: "ChecklistWorkflow");

            migrationBuilder.RenameColumn(
                name: "PunchDescription",
                table: "Punch",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Punch",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "ChecklistWorkflowId",
                table: "Punch",
                newName: "WorkflowId");

            migrationBuilder.RenameIndex(
                name: "IX_Punch_CreatedBy",
                table: "Punch",
                newName: "IX_Punch_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Punch_ChecklistWorkflowId",
                table: "Punch",
                newName: "IX_Punch_WorkflowId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Checklist",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Checklist_CreatedBy",
                table: "Checklist",
                newName: "IX_Checklist_CreatorId");

            migrationBuilder.CreateTable(
                name: "Workflow",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChecklistId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workflow_Checklist_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklist",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Workflow_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Workflow_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workflow_ChecklistId",
                table: "Workflow",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Workflow_CreatorId",
                table: "Workflow",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Workflow_UserId",
                table: "Workflow",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_User_CreatorId",
                table: "Checklist",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Punch_User_CreatorId",
                table: "Punch",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Punch_Workflow_WorkflowId",
                table: "Punch",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_User_CreatorId",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_Punch_User_CreatorId",
                table: "Punch");

            migrationBuilder.DropForeignKey(
                name: "FK_Punch_Workflow_WorkflowId",
                table: "Punch");

            migrationBuilder.DropTable(
                name: "Workflow");

            migrationBuilder.RenameColumn(
                name: "WorkflowId",
                table: "Punch",
                newName: "ChecklistWorkflowId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Punch",
                newName: "PunchDescription");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Punch",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Punch_WorkflowId",
                table: "Punch",
                newName: "IX_Punch_ChecklistWorkflowId");

            migrationBuilder.RenameIndex(
                name: "IX_Punch_CreatorId",
                table: "Punch",
                newName: "IX_Punch_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Checklist",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Checklist_CreatorId",
                table: "Checklist",
                newName: "IX_Checklist_CreatedBy");

            migrationBuilder.CreateTable(
                name: "ChecklistWorkflow",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChecklistId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistWorkflow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistWorkflow_Checklist_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklist",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChecklistWorkflow_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChecklistWorkflow_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_ChecklistId",
                table: "ChecklistWorkflow",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_CreatedById",
                table: "ChecklistWorkflow",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_UserId",
                table: "ChecklistWorkflow",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_User_CreatedBy",
                table: "Checklist",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Punch_ChecklistWorkflow_ChecklistWorkflowId",
                table: "Punch",
                column: "ChecklistWorkflowId",
                principalTable: "ChecklistWorkflow",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Punch_User_CreatedBy",
                table: "Punch",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
