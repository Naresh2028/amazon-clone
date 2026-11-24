using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazonClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orders_OrderNumner",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "OrderNumner",
                table: "orders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "OrderAmount",
                table: "orders",
                newName: "TotalAmount");

            migrationBuilder.CreateIndex(
                name: "IX_orders_TotalAmount",
                table: "orders",
                column: "TotalAmount",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orders_TotalAmount",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "orders",
                newName: "OrderAmount");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "orders",
                newName: "OrderNumner");

            migrationBuilder.CreateIndex(
                name: "IX_orders_OrderNumner",
                table: "orders",
                column: "OrderNumner",
                unique: true);
        }
    }
}
