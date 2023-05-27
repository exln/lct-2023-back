using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediWingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateErrs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiagnosisName",
                table: "InputErrors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "InputId",
                table: "InputErrors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiagnosisName",
                table: "InputErrors");

            migrationBuilder.DropColumn(
                name: "InputId",
                table: "InputErrors");
        }
    }
}
