using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OZON_Delivery_checker.Migrations
{
    /// <inheritdoc />
    public partial class addedFetchTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetchedAt",
                table: "RequestRecords",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFetchedAt",
                table: "RequestRecords");
        }
    }
}
