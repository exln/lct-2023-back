using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MediWingWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddClinic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Filial = table.Column<string>(type: "text", nullable: true),
                    DiffName = table.Column<string>(type: "text", nullable: true),
                    RateGeneral = table.Column<float>(type: "real", nullable: false),
                    RateProfes = table.Column<float>(type: "real", nullable: false),
                    RateKind = table.Column<float>(type: "real", nullable: false),
                    RateTeam = table.Column<float>(type: "real", nullable: false),
                    RateTrust = table.Column<float>(type: "real", nullable: false),
                    RatePatient = table.Column<float>(type: "real", nullable: false),
                    RateRespect = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clinics");
        }
    }
}
