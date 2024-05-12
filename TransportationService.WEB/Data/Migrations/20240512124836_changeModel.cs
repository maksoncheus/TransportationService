using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationService.WEB.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoOrders_AspNetUsers_UserId",
                table: "CargoOrders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CargoOrders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CargoOrders_UserId",
                table: "CargoOrders",
                newName: "IX_CargoOrders_CustomerId");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "TransportOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Cargos",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "CargoOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_CargoOrders_AspNetUsers_CustomerId",
                table: "CargoOrders",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoOrders_AspNetUsers_CustomerId",
                table: "CargoOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TransportOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CargoOrders");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CargoOrders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CargoOrders_CustomerId",
                table: "CargoOrders",
                newName: "IX_CargoOrders_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CargoOrders_AspNetUsers_UserId",
                table: "CargoOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
