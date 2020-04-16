using Microsoft.EntityFrameworkCore.Migrations;

namespace reservacion.Migrations
{
    public partial class CorrecionReservacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservacion_AspNetUsers_UserId1",
                table: "Reservacion");

            migrationBuilder.DropIndex(
                name: "IX_Reservacion_UserId1",
                table: "Reservacion");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reservacion");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reservacion",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reservacion_UserId",
                table: "Reservacion",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservacion_AspNetUsers_UserId",
                table: "Reservacion",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservacion_AspNetUsers_UserId",
                table: "Reservacion");

            migrationBuilder.DropIndex(
                name: "IX_Reservacion_UserId",
                table: "Reservacion");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Reservacion",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Reservacion",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservacion_UserId1",
                table: "Reservacion",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservacion_AspNetUsers_UserId1",
                table: "Reservacion",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
