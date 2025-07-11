using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlmPath.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePayPalWithStripeConnect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayPalEmail",
                table: "AspNetUsers",
                newName: "StripeConnectAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StripeConnectAccountId",
                table: "AspNetUsers",
                newName: "PayPalEmail");
        }
    }
}
