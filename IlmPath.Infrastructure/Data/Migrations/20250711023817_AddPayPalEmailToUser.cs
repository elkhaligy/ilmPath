using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlmPath.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPayPalEmailToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayPalEmail",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPalEmail",
                table: "AspNetUsers");
        }
    }
}
