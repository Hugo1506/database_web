namespace database_web.Models
{
    public class Anuncio
    {
        /// <summary>
        /// Id de um anuncio 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// preço do produto vendido no anuncio
        /// </summary>
        public string preco { get; set; }

        public string postco { get; set; }
    }
}
