using System.ComponentModel.DataAnnotations;

namespace database_web.Models
{
    public class Moderador
    {
        public Moderador()
        {
            ListaReviews = new HashSet<Review>();
            ListaAnuncio = new HashSet<Anuncio>();
        }
        /// <summary>
        /// login do moderador 
        /// </summary>
        [Key]
        public string login { get; set; }

        /// <summary>
        /// password do moderador
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// nome do moderador
        /// </summary>
        public string nome { get; set; }

        /// <summary>
        /// número de telefone do moderador
        /// </summary>
        public string telefone { get; set; }

        /// <summary>
        /// endereço email do moderador
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// quantia de dinheiro do na conta do moderador
        /// </summary>
        public decimal dinheiro { get; set; }

        /// <summary>
        /// FK para um lista de anuncios, cujo moderador fez 
        /// </summary>
        public ICollection<Anuncio> ListaAnuncio { get; set; }

        /// <summary>
        /// FK para um lista de reviews que o moderador fez
        /// </summary>
        public ICollection<Review> ListaReviews { get; set; }

    }
}
