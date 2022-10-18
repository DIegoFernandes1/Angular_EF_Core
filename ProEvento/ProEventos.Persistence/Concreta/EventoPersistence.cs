using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Concreta
{
    public class EventoPersistence : IEventoPersistence
    {
        private readonly ProEventosContext Context;

        public EventoPersistence(ProEventosContext context)
        {
            Context = context;
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = Context.Evento
                .Include(e => e.Lote)
                .Include(e => e.RedeSocial);

            if (includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(e => e.Palestrante);
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = Context.Evento
              .Include(e => e.Lote)
              .Include(e => e.RedeSocial);

            if (includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(e => e.Palestrante);
            }

            query = query.OrderBy(e => e.Id).
                    Where(e => e.Tema.ToLower().Contains(tema.ToLower())) ;

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetAllEventoByIdAsync(int idEvento, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = Context.Evento
             .Include(e => e.Lote)
             .Include(e => e.RedeSocial);

            if (includePalestrantes)
            {
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(e => e.Palestrante);
            }

            query = query.OrderBy(e => e.Id).
                    Where(e => e.Id == idEvento);

            return await query.FirstOrDefaultAsync();
        }
    }
}
