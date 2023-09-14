using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class fixPunchId1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Upload_Punch_PunchId1",
                table: "Upload");

            migrationBuilder.DropIndex(
                name: "IX_Upload_PunchId1",
                table: "Upload");

            migrationBuilder.DropColumn(
                name: "PunchId1",
                table: "Upload");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
