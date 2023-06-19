using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class taskRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Checklist_Task",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_Task_CategoryId",
                table: "Checklist_Task",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_Task_Category_CategoryId",
                table: "Checklist_Task",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_Task_Category_CategoryId",
                table: "Checklist_Task");

            migrationBuilder.DropIndex(
                name: "IX_Checklist_Task_CategoryId",
                table: "Checklist_Task");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Checklist_Task",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
