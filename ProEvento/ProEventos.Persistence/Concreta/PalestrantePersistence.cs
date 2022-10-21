using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Concreta
{
    public class PalestrantePersistence : IPalestrantesPersistence
    {
        private readonly ProEventosContext Context;

        public PalestrantePersistence(ProEventosContext context)
        {
            Context = context;
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = Context.Palestrante
            .Include(p => p.RedeSocials);

            if (includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(p => p.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id);
                    

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = Context.Palestrante
            .Include(p => p.RedeSocials);

            if (includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(p => p.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id).
                    Where(p => p.Nome.ToLower().Contains(nome.ToLower()));


            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetAllPalestranteByIdAsync(int idPalestrante, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = Context.Palestrante
          .Include(p => p.RedeSocials);

            if (includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(p => p.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                    .Where(p => p.Id == idPalestrante);


            return await query.FirstOrDefaultAsync();
        }
    }
}
