using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Concreta
{
    public class LotePersistence :  GeralPersistence, ILotePersistence
    {
        private readonly ProEventosContext proEventosContext;

        public LotePersistence(ProEventosContext proEventosContext) : base(proEventosContext)
        {
            this.proEventosContext = proEventosContext;
        }

        public async Task<Lote> GetLoteByIdsAsync(int idEvento, int id)
        {
            IQueryable<Lote> query = proEventosContext.Lote;
            query = query.AsNoTracking()
                  .Where(x => x.IdEvento == idEvento && x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByIdEventoAsync(int idEvento)
        {
            IQueryable<Lote> query = proEventosContext.Lote;

            query = query.AsNoTracking()
                .Where(x => x.IdEvento == idEvento);

            return await query.ToArrayAsync();
        }
    }
}
