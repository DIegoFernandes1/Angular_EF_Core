using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Application.Interface
{
    public interface IEventoService
    {
        Task<Evento> AddEvento(Evento model);
        Task<Evento> UpdateEvento(int idEvento, Evento model);
        Task<bool> DeleteEvento(int idEvento);

        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<Evento> GetAllEventoByIdAsync(int idEvento, bool includePalestrantes = false);
    }
}
