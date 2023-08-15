using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class ChecklistWorkflowforsendingchecklisttouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChecklistWorkflow",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkflowChecklistId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecipientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChecklistId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                        name: "FK_ChecklistWorkflow_Checklist_WorkflowChecklistId",
                        column: x => x.WorkflowChecklistId,
                        principalTable: "Checklist",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChecklistWorkflow_User_RecipientId",
                        column: x => x.RecipientId,
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
                name: "IX_ChecklistWorkflow_RecipientId",
                table: "ChecklistWorkflow",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_UserId",
                table: "ChecklistWorkflow",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistWorkflow_WorkflowChecklistId",
                table: "ChecklistWorkflow",
                column: "WorkflowChecklistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistWorkflow");
        }
    }
}
