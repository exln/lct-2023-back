using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediWingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEsilis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Msk10Standarts");

            migrationBuilder.DropTable(
                name: "Rus10Standarts");

            migrationBuilder.CreateTable(
                name: "Mkb10Standarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EsiliName = table.Column<string>(type: "text", nullable: false),
                    Mkb10Code = table.Column<string>(type: "text", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mkb10Standarts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mkb10Standarts");

            migrationBuilder.CreateTable(
                name: "Msk10Standarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Mkb10Code = table.Column<string>(type: "text", nullable: false),
                    MskEsiliCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Msk10Standarts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rus10Standarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Mkb10Code = table.Column<string>(type: "text", nullable: false),
                    Probability = table.Column<float>(type: "real", nullable: false),
                    RusEsiliCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rus10Standarts", x => x.Id);
                });
        }
    }
}
