using ProEventos.Domain.Models;
using ProEventos.Persistence.Data;
using System.Collections.Generic;
using System.Linq;

namespace ProEventos.Application.Services
{
    public class EventoService
    {
        private readonly ProEventosContext _context;

        public EventoService(ProEventosContext dataContext)
        {
            _context = dataContext;
        }

        public IEnumerable<Evento> Get()
        {
            return _context.Evento;
        }

        public Evento Get(int id)
        {
            return _context.Evento.FirstOrDefault(x => x.Id == id);
        }
    }
}
