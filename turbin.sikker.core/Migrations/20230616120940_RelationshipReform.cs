using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipReform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistToTaskLink_Checklist_ChecklistsId",
                table: "ChecklistToTaskLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChecklistToTaskLink",
                table: "ChecklistToTaskLink");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistToTaskLink_ChecklistsId",
                table: "ChecklistToTaskLink");

            migrationBuilder.RenameColumn(
                name: "ChecklistsId",
                table: "ChecklistToTaskLink",
                newName: "ChecklistId");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Checklist",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChecklistToTaskLink",
                table: "ChecklistToTaskLink",
                columns: new[] { "ChecklistId", "ChecklistTasksId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistToTaskLink_ChecklistTasksId",
                table: "ChecklistToTaskLink",
                column: "ChecklistTasksId");

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_CreatedBy",
                table: "Checklist",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_User_CreatedBy",
                table: "Checklist",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistToTaskLink_Checklist_ChecklistId",
                table: "ChecklistToTaskLink",
                column: "ChecklistId",
                principalTable: "Checklist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_User_CreatedBy",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistToTaskLink_Checklist_ChecklistId",
                table: "ChecklistToTaskLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChecklistToTaskLink",
                table: "ChecklistToTaskLink");

            migrationBuilder.DropIndex(
                name: "IX_ChecklistToTaskLink_ChecklistTasksId",
                table: "ChecklistToTaskLink");

            migrationBuilder.DropIndex(
                name: "IX_Checklist_CreatedBy",
                table: "Checklist");

            migrationBuilder.RenameColumn(
                name: "ChecklistId",
                table: "ChecklistToTaskLink",
                newName: "ChecklistsId");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Checklist",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChecklistToTaskLink",
                table: "ChecklistToTaskLink",
                columns: new[] { "ChecklistTasksId", "ChecklistsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistToTaskLink_ChecklistsId",
                table: "ChecklistToTaskLink",
                column: "ChecklistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistToTaskLink_Checklist_ChecklistsId",
                table: "ChecklistToTaskLink",
                column: "ChecklistsId",
                principalTable: "Checklist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
