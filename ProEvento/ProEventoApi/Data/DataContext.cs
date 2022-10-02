using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProEventos.Api.Models;

namespace ProEventos.Api.Data
{
    public class DataContext: DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("ProEventoDatabase"));
        }

        public DbSet<Evento> Evento { get; set; }
    }
}
