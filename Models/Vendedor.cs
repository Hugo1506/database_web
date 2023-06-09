﻿using System.ComponentModel.DataAnnotations;

namespace database_web.Models
{
    public class Vendedor
    {
        public Vendedor()
        {
            ListaAnuncio = new HashSet<Anuncio>();
        }
        /// <summary>
        /// login do vendedor 
        /// </summary>
        [Key]
        public string login { get; set; }

        /// <summary>
        /// password do vendedor
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// nome do vendedor
        /// </summary>
        public string nome { get; set; }

        /// <summary>
        /// número de telefone do vendedor
        /// </summary>
        public string telefone { get; set; }

        /// <summary>
        /// endereço email do vendedor
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// quantia de dinheiro do na conta do vendedor
        /// </summary>
        public decimal dinheiro { get; set; }

        /// <summary>
        /// FK para um lista de anuncios, cujo vendedor fez 
        /// </summary>
        public ICollection<Anuncio> ListaAnuncio { get; set; }
    }
}
