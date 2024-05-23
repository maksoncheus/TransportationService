using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationService.WEB.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDateTimeFormatForStoringItProperly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDateTime",
                table: "CargoOrders");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DeliveryDate",
                table: "TransportOrders",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DeliveryMaxTime",
                table: "TransportOrders",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DeliveryMinTime",
                table: "TransportOrders",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DeliveryDate",
                table: "CargoOrders",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DeliveryMaxTime",
                table: "CargoOrders",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "DeliveryMinTime",
                table: "CargoOrders",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryMaxTime",
                table: "TransportOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryMinTime",
                table: "TransportOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "CargoOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryMaxTime",
                table: "CargoOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryMinTime",
                table: "CargoOrders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDate",
                table: "TransportOrders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDateTime",
                table: "CargoOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
