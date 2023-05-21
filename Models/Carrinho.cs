namespace database_web.Models
{
    public class Carrinho
    {
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
        /// FK para o comprador, dono do carriho 
        /// </summary>
        public Comprador comprador { get; set; }
    }
}
