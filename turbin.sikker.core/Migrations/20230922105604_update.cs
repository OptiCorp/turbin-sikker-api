using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "HoursSpent",
                table: "Workflow",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgHoursSpent",
                table: "Checklist",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursSpent",
                table: "Workflow");

            migrationBuilder.DropColumn(
                name: "AvgHoursSpent",
                table: "Checklist");
        }
    }
}
