using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediWingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddErrors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTagId",
                table: "Standarts");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Standarts");

            migrationBuilder.CreateTable(
                name: "InputErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputErrors", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputErrors");

            migrationBuilder.AddColumn<int>(
                name: "SubTagId",
                table: "Standarts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Standarts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
