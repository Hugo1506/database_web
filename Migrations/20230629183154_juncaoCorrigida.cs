using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database_web.Migrations
{
    /// <inheritdoc />
    public partial class juncaoCorrigida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comprador_produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompradorLogin = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comprador_produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comprador_produto_comprador_CompradorLogin",
                        column: x => x.CompradorLogin,
                        principalTable: "comprador",
                        principalColumn: "login",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comprador_produto_produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comprador_produto_CompradorLogin",
                table: "comprador_produto",
                column: "CompradorLogin");

            migrationBuilder.CreateIndex(
                name: "IX_comprador_produto_ProdutoId",
                table: "comprador_produto",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comprador_produto");
        }
    }
}
