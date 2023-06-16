using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class ChecklistRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChecklistToTaskLink",
                columns: table => new
                {
                    ChecklistTasksId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChecklistsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistToTaskLink", x => new { x.ChecklistTasksId, x.ChecklistsId });
                    table.ForeignKey(
                        name: "FK_ChecklistToTaskLink_Checklist_ChecklistsId",
                        column: x => x.ChecklistsId,
                        principalTable: "Checklist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistToTaskLink_Checklist_Task_ChecklistTasksId",
                        column: x => x.ChecklistTasksId,
                        principalTable: "Checklist_Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistToTaskLink_ChecklistsId",
                table: "ChecklistToTaskLink",
                column: "ChecklistsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistToTaskLink");
        }
    }
}
