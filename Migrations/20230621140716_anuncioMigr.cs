using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database_web.Migrations
{
    /// <inheritdoc />
    public partial class anuncioMigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_anuncio_moderador_moderadorlogin",
                table: "anuncio");

            migrationBuilder.DropForeignKey(
                name: "FK_anuncio_vendedor_vendedorlogin",
                table: "anuncio");

            migrationBuilder.AlterColumn<string>(
                name: "vendedorlogin",
                table: "anuncio",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "moderadorlogin",
                table: "anuncio",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "VendedorFK",
                table: "anuncio",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ModeradorFK",
                table: "anuncio",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_anuncio_moderador_moderadorlogin",
                table: "anuncio",
                column: "moderadorlogin",
                principalTable: "moderador",
                principalColumn: "login");

            migrationBuilder.AddForeignKey(
                name: "FK_anuncio_vendedor_vendedorlogin",
                table: "anuncio",
                column: "vendedorlogin",
                principalTable: "vendedor",
                principalColumn: "login");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_anuncio_moderador_moderadorlogin",
                table: "anuncio");

            migrationBuilder.DropForeignKey(
                name: "FK_anuncio_vendedor_vendedorlogin",
                table: "anuncio");

            migrationBuilder.AlterColumn<string>(
                name: "vendedorlogin",
                table: "anuncio",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "moderadorlogin",
                table: "anuncio",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VendedorFK",
                table: "anuncio",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModeradorFK",
                table: "anuncio",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_anuncio_moderador_moderadorlogin",
                table: "anuncio",
                column: "moderadorlogin",
                principalTable: "moderador",
                principalColumn: "login",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_anuncio_vendedor_vendedorlogin",
                table: "anuncio",
                column: "vendedorlogin",
                principalTable: "vendedor",
                principalColumn: "login",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
