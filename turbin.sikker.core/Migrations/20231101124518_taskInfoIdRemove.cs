using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class taskInfoIdRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskInfo",
                table: "TaskInfo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TaskInfo");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "TaskInfo",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskInfo",
                table: "TaskInfo",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskInfo",
                table: "TaskInfo");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "TaskInfo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "TaskInfo",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskInfo",
                table: "TaskInfo",
                column: "Id");
        }
    }
}
