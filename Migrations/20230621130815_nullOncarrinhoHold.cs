using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace database_web.Migrations
{
    /// <inheritdoc />
    public partial class nullOncarrinhoHold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produto_carrinho_carrinhoId",
                table: "produto");

            migrationBuilder.AlterColumn<int>(
                name: "carrinhoId",
                table: "produto",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_produto_carrinho_carrinhoId",
                table: "produto",
                column: "carrinhoId",
                principalTable: "carrinho",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produto_carrinho_carrinhoId",
                table: "produto");

            migrationBuilder.AlterColumn<int>(
                name: "carrinhoId",
                table: "produto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_produto_carrinho_carrinhoId",
                table: "produto",
                column: "carrinhoId",
                principalTable: "carrinho",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
