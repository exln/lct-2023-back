using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediWingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "HealthcareServices");

            migrationBuilder.DropTable(
                name: "ServiceBlocks");

            migrationBuilder.DropTable(
                name: "ServiceNumbers");

            migrationBuilder.DropTable(
                name: "ServiceSections");

            migrationBuilder.DropTable(
                name: "Standarts");

            migrationBuilder.CreateTable(
                name: "Mkb10Chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Chapter = table.Column<string>(type: "text", nullable: false),
                    Sub = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mkb10Chapters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Msk10Standarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MskEsiliCode = table.Column<string>(type: "text", nullable: false),
                    Mkb10Code = table.Column<string>(type: "text", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Msk10Standarts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MskEsilis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MskEsiliCode = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsChild = table.Column<bool>(type: "boolean", nullable: true),
                    IdCode = table.Column<int>(type: "integer", nullable: true),
                    LdpCode = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Ambulatory = table.Column<bool>(type: "boolean", nullable: true),
                    Stationary = table.Column<bool>(type: "boolean", nullable: true),
                    Modalities = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MskEsilis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MskEsiliTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MskEsiliTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rus10Standarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RusEsiliCode = table.Column<string>(type: "text", nullable: false),
                    Mkb10Code = table.Column<string>(type: "text", nullable: false),
                    Probability = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rus10Standarts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RusEsiliBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Section = table.Column<char>(type: "character(1)", nullable: false),
                    Block = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RusEsiliBlocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RusEsiliNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Section = table.Column<char>(type: "character(1)", nullable: false),
                    Block = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RusEsiliNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RusEsilis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Section = table.Column<char>(type: "character(1)", nullable: false),
                    Block = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Subnumber = table.Column<int>(type: "integer", nullable: false),
                    Subsubnumber = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RusEsilis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RusEsiliSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Section = table.Column<char>(type: "character(1)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RusEsiliSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MskEsiliAnalog",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    MskEsiliId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MskEsiliAnalog", x => x.Name);
                    table.ForeignKey(
                        name: "FK_MskEsiliAnalog_MskEsilis_MskEsiliId",
                        column: x => x.MskEsiliId,
                        principalTable: "MskEsilis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MskEsiliAnalog_MskEsiliId",
                table: "MskEsiliAnalog",
                column: "MskEsiliId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mkb10Chapters");

            migrationBuilder.DropTable(
                name: "Msk10Standarts");

            migrationBuilder.DropTable(
                name: "MskEsiliAnalog");

            migrationBuilder.DropTable(
                name: "MskEsiliTypes");

            migrationBuilder.DropTable(
                name: "Rus10Standarts");

            migrationBuilder.DropTable(
                name: "RusEsiliBlocks");

            migrationBuilder.DropTable(
                name: "RusEsiliNumbers");

            migrationBuilder.DropTable(
                name: "RusEsilis");

            migrationBuilder.DropTable(
                name: "RusEsiliSections");

            migrationBuilder.DropTable(
                name: "MskEsilis");

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Chapter = table.Column<string>(type: "text", nullable: false),
                    Sub = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "HealthcareServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Block = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Section = table.Column<char>(type: "character(1)", nullable: false),
                    Subnumber = table.Column<int>(type: "integer", nullable: false),
                    Subsubnumber = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthcareServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Block = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Section = table.Column<char>(type: "character(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceBlocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Block = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Section = table.Column<char>(type: "character(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Section = table.Column<char>(type: "character(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Standarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HealthcareServiceCode = table.Column<string>(type: "text", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Mkb10Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standarts", x => x.Id);
                });
        }
    }
}
