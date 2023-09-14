using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upload_Punch_PunchId",
                table: "Upload");

            migrationBuilder.AddColumn<string>(
                name: "PunchId1",
                table: "Upload",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Upload_PunchId1",
                table: "Upload",
                column: "PunchId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Upload_Punch_PunchId",
                table: "Upload",
                column: "PunchId",
                principalTable: "Punch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Upload_Punch_PunchId1",
                table: "Upload",
                column: "PunchId1",
                principalTable: "Punch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upload_Punch_PunchId",
                table: "Upload");

            migrationBuilder.DropForeignKey(
                name: "FK_Upload_Punch_PunchId1",
                table: "Upload");

            migrationBuilder.DropIndex(
                name: "IX_Upload_PunchId1",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "PunchId1",
                table: "Upload");

            migrationBuilder.AddForeignKey(
                name: "FK_Upload_Punch_PunchId",
                table: "Upload",
                column: "PunchId",
                principalTable: "Punch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
