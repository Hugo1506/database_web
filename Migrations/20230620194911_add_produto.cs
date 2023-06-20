using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database_web.Migrations
{
    /// <inheritdoc />
    public partial class add_produto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdutoFK",
                table: "anuncio",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_anuncio_ProdutoFK",
                table: "anuncio",
                column: "ProdutoFK");

            migrationBuilder.AddForeignKey(
                name: "FK_anuncio_produto_ProdutoFK",
                table: "anuncio",
                column: "ProdutoFK",
                principalTable: "produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_anuncio_produto_ProdutoFK",
                table: "anuncio");

            migrationBuilder.DropIndex(
                name: "IX_anuncio_ProdutoFK",
                table: "anuncio");

            migrationBuilder.DropColumn(
                name: "ProdutoFK",
                table: "anuncio");
        }
    }
}
