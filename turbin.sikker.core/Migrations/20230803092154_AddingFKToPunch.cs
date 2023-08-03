using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class AddingFKToPunch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Punch_CreatedBy",
                table: "Punch",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Punch_User_CreatedBy",
                table: "Punch",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Punch_User_CreatedBy",
                table: "Punch");

            migrationBuilder.DropIndex(
                name: "IX_Punch_CreatedBy",
                table: "Punch");
        }
    }
}
