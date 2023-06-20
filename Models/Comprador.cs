﻿using System.ComponentModel.DataAnnotations;

namespace database_web.Models
{
    public class Comprador
    {
        public Comprador()
        {
            ListaReviews = new HashSet<Review>();
            ListaCarrinhos = new HashSet<Carrinho>();
            ListaProdutos = new HashSet<Produto>();
        }

        /// <summary>
        /// login do comprador 
        /// </summary>
        [Key]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]

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
 
        [EmailAddress]
        public string email { get; set; }

        /// <summary>
        /// quantia de dinheiro do na conta do comprador
        /// </summary>
        public int dinheiro { get; set; }

        /// <summary>
        /// atributo para efetuar a ligação entre a base 
        /// de dados do 'negócio' e a base de dados da autenticação
        /// </summary>

        public string UserId { get; set; }

        /// <summary>
        /// Lista carrinho do comprador
        /// </summary>
        public ICollection<Carrinho> ListaCarrinhos { get; set; }

        /// <summary>
        /// FK para a lista dos produtos que o comprador comprou
        /// </summary>
        public ICollection<Produto> ListaProdutos { get; set; }


        /// <summary>
        /// FK para a lista de reviews que o comprador fez
        /// </summary>
        public ICollection<Review> ListaReviews { get; set; }


    }
}
