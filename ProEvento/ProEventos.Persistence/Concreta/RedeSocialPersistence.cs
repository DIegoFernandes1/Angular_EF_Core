using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Concreta
{
    public class RedeSocialPersistence : GeralPersistence, IRedeSocialPersistence
    {
        private readonly ProEventosContext proEventosContext;

        public RedeSocialPersistence(ProEventosContext proEventosContext) : base(proEventosContext)
        {
            this.proEventosContext = proEventosContext;
        }

        public async Task<RedeSocial[]> GetAllByEventoIdAsync(int eventoId)
        {
            IQueryable<RedeSocial> query = proEventosContext.redeSocial;

            query = query.AsNoTracking()
                .Where(rs => rs.IdEvento == eventoId);

            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            IQueryable<RedeSocial> query = proEventosContext.redeSocial;

            query = query.AsNoTracking()
                .Where(rs => rs.IdPalestrante == palestranteId);

            return await query.ToArrayAsync();
        }

        public async Task<RedeSocial> GetRedeSocialEventoByIdsAsync(int eventoId, int id)
        {
            IQueryable<RedeSocial> query = proEventosContext.redeSocial;

            query = query.AsNoTracking()
                .Where(rs => rs.IdEvento == eventoId && rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<RedeSocial> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id)
        {
            IQueryable<RedeSocial> query = proEventosContext.redeSocial;

            query = query.AsNoTracking()
                .Where(rs => rs.IdPalestrante == palestranteId && rs.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}