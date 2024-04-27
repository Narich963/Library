using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlWork7.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPublishedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "PublishedYear",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedYear",
                table: "Books");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "Books",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
