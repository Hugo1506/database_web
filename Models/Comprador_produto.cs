using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace database_web.Models
{
    public class Comprador_produto
    {
        [Key]
        public int Id { get; set; }
        public string CompradorLogin { get; set; }
        public Comprador comprador { get; set; }

        public int ProdutoId { get; set; }
        public Produto produto { get; set; }
    }
}
