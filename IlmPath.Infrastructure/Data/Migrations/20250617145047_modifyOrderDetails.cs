using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlmPath.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class modifyOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_AspNetUsers_ApplicationUserId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ApplicationUserId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_EnrollmentId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_EnrollmentId",
                table: "OrderDetails",
                column: "EnrollmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_EnrollmentId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "OrderDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ApplicationUserId",
                table: "OrderDetails",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_EnrollmentId",
                table: "OrderDetails",
                column: "EnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_AspNetUsers_ApplicationUserId",
                table: "OrderDetails",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
