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
        /// FK do comprador que realizou a review
        /// </summary>
        public Comprador comprador { get; set; }
    }
}
