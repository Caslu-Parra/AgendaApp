using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaApp.Migrations
{
    /// <inheritdoc />
    public partial class CargaEntityCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dtUltVisita",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "dtInclusao",
                table: "Pets",
                newName: "DtInclusao");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Pets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CPF = table.Column<string>(type: "TEXT", nullable: false),
                    DtInclusao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    PetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ClienteId",
                table: "Pets",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CPF",
                table: "Clientes",
                column: "CPF",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Clientes_ClienteId",
                table: "Pets",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Clientes_ClienteId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Pets_ClienteId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "DtInclusao",
                table: "Pets",
                newName: "dtInclusao");

            migrationBuilder.AddColumn<DateTime>(
                name: "dtUltVisita",
                table: "Pets",
                type: "TEXT",
                nullable: true);
        }
    }
}
