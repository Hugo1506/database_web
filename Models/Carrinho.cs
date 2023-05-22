using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace database_web.Models
{
    public class Carrinho
    {
        public Carrinho() {
            ListaProdutos = new HashSet<Produto>();
        }
        /// <summary>
        /// Id do carrinho
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// nome do carrinho
        /// </summary>
        public string nome { get; set; }
        /// <summary>
        /// preço total do carrinho
        /// </summary>
        public decimal preco { get; set; }

        /// <summary>
        /// FK para o comprador dono do carrinho
        /// </summary>
        [ForeignKey(nameof(Comprador))] 
        public int CompradorFK { get; set; }
        public Comprador comprador { get; set; }

        /// <summary>
        /// Lista de produtos de um carrinho
        /// </summary>
        public ICollection<Produto> ListaProdutos { get; set; }

    }
}
