using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turbin.sikker.core.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceId",
                table: "Workflow",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workflow_InvoiceId",
                table: "Workflow",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workflow_Invoice_InvoiceId",
                table: "Workflow",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workflow_Invoice_InvoiceId",
                table: "Workflow");

            migrationBuilder.DropIndex(
                name: "IX_Workflow_InvoiceId",
                table: "Workflow");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Workflow");
        }
    }
}
