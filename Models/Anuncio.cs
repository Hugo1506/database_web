using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace database_web.Models
{
    public class Anuncio
    {
        public Anuncio() {
            ListaReviews = new HashSet<Review>();
        }
        /// <summary>
        /// Id de um anuncio 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// preço do produto vendido no anuncio
        /// </summary>
        public string preco { get; set; }

        /// <summary>
        /// FK para o Moderador que apaga o anuncio
        /// </summary>
        [ForeignKey(nameof(Moderador))]
        [Display(Name = "Moderador")]
        public int ModeradorFK { get; set; }
        public Moderador moderador { get; set; }

        /// <summary>
        /// FK para o vendedor que fez o anuncio 
        /// </summary>
        [ForeignKey(nameof(Vendedor))]
        [Display(Name = "Vendedor")]
        public int VendedorFK { get; set; }
        public Vendedor vendedor { get; set; }

        /// <summary>
        /// FK para um lista de reviews pertencentes ao anuncio
        /// </summary>
        public ICollection<Review> ListaReviews { get; set; }
    }
}
