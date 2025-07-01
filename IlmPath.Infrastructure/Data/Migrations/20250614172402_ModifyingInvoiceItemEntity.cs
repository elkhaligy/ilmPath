using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlmPath.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingInvoiceItemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineTotal",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InvoiceItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LineTotal",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
