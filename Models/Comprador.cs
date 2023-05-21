namespace database_web.Models
{
    public class Comprador
    {
        public Comprador()
        {
            ListaReviews = new HashSet<Review>();
        }

        /// <summary>
        /// login do comprador 
        /// </summary>
        public string login { get; set; }

        /// <summary>
        /// password do comprador
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// nome do comprador
        /// </summary>
        public string nome { get; set; }

        /// <summary>
        /// número de telefone do comprador
        /// </summary>
        public string telefone { get; set; }

        /// <summary>
        /// endereço email do comprador
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// quantia de dinheiro do na conta do comprador
        /// </summary>
        public int dinheiro { get; set; }

        /// <summary>
        /// FK para um lista de reviews que o comprador fez
        /// </summary>
        public ICollection<Review> ListaReviews { get; set; }


    }
}
