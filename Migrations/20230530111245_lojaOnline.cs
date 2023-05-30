using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database_web.Migrations
{
    /// <inheritdoc />
    public partial class lojaOnline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "comprador",
                columns: table => new
                {
                    login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dinheiro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comprador", x => x.login);
                });

            migrationBuilder.CreateTable(
                name: "moderador",
                columns: table => new
                {
                    login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dinheiro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderador", x => x.login);
                });

            migrationBuilder.CreateTable(
                name: "vendedor",
                columns: table => new
                {
                    login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dinheiro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendedor", x => x.login);
                });

            migrationBuilder.CreateTable(
                name: "carrinho",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    preco = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CompradorFK = table.Column<int>(type: "int", nullable: false),
                    compradorlogin = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrinho", x => x.Id);
                    table.ForeignKey(
                        name: "FK_carrinho_comprador_compradorlogin",
                        column: x => x.compradorlogin,
                        principalTable: "comprador",
                        principalColumn: "login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anuncio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    preco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModeradorFK = table.Column<int>(type: "int", nullable: false),
                    moderadorlogin = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VendedorFK = table.Column<int>(type: "int", nullable: false),
                    vendedorlogin = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anuncio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_anuncio_moderador_moderadorlogin",
                        column: x => x.moderadorlogin,
                        principalTable: "moderador",
                        principalColumn: "login",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_anuncio_vendedor_vendedorlogin",
                        column: x => x.vendedorlogin,
                        principalTable: "vendedor",
                        principalColumn: "login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "produto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompradorFK = table.Column<int>(type: "int", nullable: false),
                    compradorlogin = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CarrinhoFK = table.Column<int>(type: "int", nullable: false),
                    carrinhoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_produto_carrinho_carrinhoId",
                        column: x => x.carrinhoId,
                        principalTable: "carrinho",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_produto_comprador_compradorlogin",
                        column: x => x.compradorlogin,
                        principalTable: "comprador",
                        principalColumn: "login");
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompradorFK = table.Column<int>(type: "int", nullable: false),
                    compradorlogin = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AnuncioFK = table.Column<int>(type: "int", nullable: false),
                    anuncioId = table.Column<int>(type: "int", nullable: false),
                    ModeradorFK = table.Column<int>(type: "int", nullable: false),
                    moderadorlogin = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_anuncio_anuncioId",
                        column: x => x.anuncioId,
                        principalTable: "anuncio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_comprador_compradorlogin",
                        column: x => x.compradorlogin,
                        principalTable: "comprador",
                        principalColumn: "login");
                    table.ForeignKey(
                        name: "FK_review_moderador_moderadorlogin",
                        column: x => x.moderadorlogin,
                        principalTable: "moderador",
                        principalColumn: "login");
                });

            migrationBuilder.CreateIndex(
                name: "IX_anuncio_moderadorlogin",
                table: "anuncio",
                column: "moderadorlogin");

            migrationBuilder.CreateIndex(
                name: "IX_anuncio_vendedorlogin",
                table: "anuncio",
                column: "vendedorlogin");

            migrationBuilder.CreateIndex(
                name: "IX_carrinho_compradorlogin",
                table: "carrinho",
                column: "compradorlogin");

            migrationBuilder.CreateIndex(
                name: "IX_produto_carrinhoId",
                table: "produto",
                column: "carrinhoId");

            migrationBuilder.CreateIndex(
                name: "IX_produto_compradorlogin",
                table: "produto",
                column: "compradorlogin");

            migrationBuilder.CreateIndex(
                name: "IX_review_anuncioId",
                table: "review",
                column: "anuncioId");

            migrationBuilder.CreateIndex(
                name: "IX_review_compradorlogin",
                table: "review",
                column: "compradorlogin");

            migrationBuilder.CreateIndex(
                name: "IX_review_moderadorlogin",
                table: "review",
                column: "moderadorlogin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produto");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "carrinho");

            migrationBuilder.DropTable(
                name: "anuncio");

            migrationBuilder.DropTable(
                name: "comprador");

            migrationBuilder.DropTable(
                name: "moderador");

            migrationBuilder.DropTable(
                name: "vendedor");
        }
    }
}
