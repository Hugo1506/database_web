using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace database_web.Models
{
    public class Produto
    {
        /// <summary>
        /// Id do um produto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// nome de um produto 
        /// </summary>
        public string nome { get; set; }

        /// <summary>
        /// descrição de um produto
        /// </summary>
        public string descricao { get; set; }

        /// <summary>
        /// FK para o comprador que faz a compra do produto 
        /// </summary>
        [ForeignKey(nameof(Comprador))]
        [Display(Name = "Comprador")]
        public int? CompradorFK { get; set; }
        public Comprador? comprador { get; set; }

        /// <summary>
        /// FK para o carrinho que contém o produto
        /// </summary>
        [ForeignKey(nameof(Carrinho))]
        [Display(Name = "Carrinho")] 
        public int? CarrinhoFK { get; set; }
        public Carrinho? carrinho { get; set; }
    }
}
