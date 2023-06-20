using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace database_web.Models
{
    public class Review
    {
        /// <summary>
        /// Id de um review
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// conteudo de um review 
        /// </summary>
        public string conteudo { get; set; }

        /// <summary>
        /// FK para o comprador que fez a review 
        /// </summary>
        [ForeignKey(nameof(Comprador))]
        [Display(Name = "Comprador")]
        public int CompradorFK { get; set; }
        public Comprador comprador { get; set; }

        /// <summary>
        /// FK para o anuncio a que a review pertence
        /// </summary>
        [ForeignKey(nameof(Anuncio))]
        [Display(Name = "Anuncio")]
        public int AnuncioFK { get; set; }
        public Anuncio anuncio { get; set; }

        /// <summary>
        /// FK para o moderador que apaga a review 
        /// </summary>
        [ForeignKey(nameof(Moderador))]
        [Display(Name = "Moderador")]
        public int ModeradorFK { get; set; }
        public Moderador moderador { get; set; }
    }
}
