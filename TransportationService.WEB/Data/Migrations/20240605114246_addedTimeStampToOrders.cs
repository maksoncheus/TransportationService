﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationService.WEB.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedTimeStampToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "TransportOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "CargoOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "TransportOrders");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "CargoOrders");
        }
    }
}
