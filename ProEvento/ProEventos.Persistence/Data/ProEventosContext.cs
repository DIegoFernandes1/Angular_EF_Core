using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Data
{
    public class ProEventosContext : IdentityDbContext<User, Role, int,
                                                      IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                                      IdentityRoleClaim<int>, IdentityUserToken<int>>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                userRole.HasOne(ur => ur.User)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();
            });

            modelBuilder.Entity<Evento>()
                 .HasMany(e => e.RedeSocial)
                 .WithOne(rs => rs.Evento)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Evento>()
               .HasMany(l => l.Lote)
               .WithOne(e => e.Evento)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Palestrante>()
                .HasMany(e => e.RedeSocials)
                .WithOne(rs => rs.Palestrante)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
