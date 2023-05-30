using database_web.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace database_web.Data
{

    /// <summary>
    /// esta classe representa a Base de Dados do nosso projeto
    /// </summary>
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /* *********************************************
         * Criação das Tabelas
         * ********************************************* */
        public DbSet<Anuncio> anuncio { get; set; }
        public DbSet<Carrinho> carrinho { get; set; }
        public DbSet<Comprador> comprador { get; set; }
        public DbSet<Moderador> moderador { get; set; }
        public DbSet<Produto> produto { get; set; }
        public DbSet<Review> review { get; set; }
        public DbSet<Vendedor> vendedor { get; set; }

    }
}