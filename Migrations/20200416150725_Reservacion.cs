using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace reservacion.Migrations
{
    public partial class Reservacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Sala",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "descripcionUbicacion",
                table: "Sala",
                newName: "DescripcionUbicacion");

            migrationBuilder.CreateTable(
                name: "Reservacion",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaReservacion = table.Column<DateTime>(nullable: false),
                    ReservacionDesde = table.Column<DateTime>(nullable: false),
                    ReservacionHasta = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true),
                    SalaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservacion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reservacion_Sala_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Sala",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservacion_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservacion_SalaId",
                table: "Reservacion",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservacion_UserId1",
                table: "Reservacion",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservacion");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Sala",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "DescripcionUbicacion",
                table: "Sala",
                newName: "descripcionUbicacion");
        }
    }
}
