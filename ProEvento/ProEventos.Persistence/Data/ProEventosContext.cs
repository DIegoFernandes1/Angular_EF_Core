using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Data
{
    public class ProEventosContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ProEventosContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("ProEventoDatabase"));
        }

        public DbSet<Evento> Evento { get; set; }
        public DbSet<Lote> Lote { get; set; }
        public DbSet<Palestrante> Palestrante { get; set; }
        public DbSet<RedeSocial> redeSocial { get; set; }
        public DbSet<PalestranteEvento> palestranteEvento { get; set; }

        protected void onModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(PE => new { PE.IdEvento, PE.IdPalestrante });
        }
    }
}
