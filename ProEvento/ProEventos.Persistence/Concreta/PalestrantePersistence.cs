using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Domain.Models.Enum;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Concreta
{
    public class PalestrantePersistence : GeralPersistence, IPalestrantesPersistence
    {
        private readonly ProEventosContext Context;

        public PalestrantePersistence(ProEventosContext context) : base(context)
        {
            Context = context;
        }

        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = Context.Palestrante
             .Include(p => p.User)
             .Include(p => p.RedeSocials);

            if (includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(p => p.Evento);
            }

            query = query.AsNoTracking()
                 .Where(p =>
                     (p.MiniCurriculo.ToLower().Contains(pageParams.Term.ToLower())
                        || p.User.Nome.ToLower().Contains(pageParams.Term.ToLower())
                        || p.User.Sobrenome.ToLower().Contains(pageParams.Term.ToLower()))
                     && p.User.Funcao == Funcao.Palestrante)
                .OrderBy(p => p.Id);



            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Palestrante> GetAllPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = Context.Palestrante
                .Include(p => p.User)
                .Include(p => p.RedeSocials);

            if (includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(p => p.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                    .Where(p => p.UserId == userId);


            return await query.FirstOrDefaultAsync();
        }
    }
}
