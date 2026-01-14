using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dj_backend.Migrations
{
    /// <inheritdoc />
    public partial class FirstSeedEntryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Entries",
                columns: new[] { "Id", "Date", "GotSick", "Ingredients", "Title" },
                values: new object[] { 1, new DateOnly(2024, 1, 15), false, new List<string> { "Cheese", "Tomatoes" }, "Pizza" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Entries",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
