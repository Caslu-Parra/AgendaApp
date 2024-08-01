using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaApp.Migrations
{
    /// <inheritdoc />
    public partial class CargaEntity_MedicoConsultaAtendimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    DtConsulta = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    IdPet = table.Column<int>(type: "INTEGER", nullable: false),
                    IdAtendimento = table.Column<int>(type: "INTEGER", nullable: true),
                    DtInclusao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => new { x.Id, x.DtConsulta, x.IdPet });
                    table.UniqueConstraint("AK_Consultas_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultas_Pets_IdPet",
                        column: x => x.IdPet,
                        principalTable: "Pets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Medicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CRM = table.Column<string>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    CPF = table.Column<string>(type: "TEXT", nullable: false),
                    DtInclusao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Atendimentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdConsulta = table.Column<int>(type: "INTEGER", nullable: false),
                    IdMedicoResp = table.Column<int>(type: "INTEGER", nullable: false),
                    Anotacoes = table.Column<string>(type: "TEXT", nullable: false),
                    DtInclusao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atendimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Consultas_IdConsulta",
                        column: x => x.IdConsulta,
                        principalTable: "Consultas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Medicos_IdMedicoResp",
                        column: x => x.IdMedicoResp,
                        principalTable: "Medicos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_IdConsulta",
                table: "Atendimentos",
                column: "IdConsulta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_IdMedicoResp",
                table: "Atendimentos",
                column: "IdMedicoResp");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_IdAtendimento",
                table: "Consultas",
                column: "IdAtendimento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_IdPet",
                table: "Consultas",
                column: "IdPet");

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_CPF",
                table: "Medicos",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_CRM",
                table: "Medicos",
                column: "CRM",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atendimentos");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Medicos");
        }
    }
}
