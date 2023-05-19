using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediWingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddInputTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDiagnosticInputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InputId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sex = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PatientId = table.Column<int>(type: "integer", nullable: true),
                    MKBCode = table.Column<string>(type: "text", nullable: true),
                    Diagnosis = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    DoctorPost = table.Column<string>(type: "text", nullable: true),
                    Recomendation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiagnosticInputs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInputRelations",
                columns: table => new
                {
                    InputId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InputName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInputRelations", x => x.InputId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDiagnosticInputs");

            migrationBuilder.DropTable(
                name: "UserInputRelations");
        }
    }
}
