using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Concreta
{
    public class EventoPersistence : GeralPersistence, IEventoPersistence
    {
        private readonly ProEventosContext Context;

        public EventoPersistence(ProEventosContext context) : base(context)
        {
            Context = context;
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int idUser, PageParams pageParams, bool includePalestrantes = false)
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

            query = query.AsNoTracking()
                 .Where(e => (e.Tema.ToLower().Contains(pageParams.Term.ToLower())
                                || e.Local.ToLower().Contains(pageParams.Term.ToLower()))
                            && e.UserId == idUser)
                .OrderBy(e => e.Id);

            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }

        public async Task<Evento> GetAllEventoByIdAsync(int idUser, int idEvento, bool includePalestrantes = false)
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

            query = query.AsNoTracking().OrderBy(e => e.Id).
                    Where(e => e.Id == idEvento && e.UserId == idUser);

            return await query.FirstOrDefaultAsync();
        }
    }
}
