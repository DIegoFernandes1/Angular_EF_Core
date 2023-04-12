using ProEventos.Application.DTOs;
using System.Threading.Tasks;

namespace ProEventos.Application.Interface
{
    public interface IEventoService
    {
        Task<EventoDTO> AddEvento(int idUser, EventoDTO model);
        Task<EventoDTO> UpdateEvento(int idUser, int idEvento, EventoDTO model);
        Task<bool> DeleteEvento(int idUser, int idEvento);

        Task<EventoDTO[]> GetAllEventosAsync(int idUser, bool includePalestrantes = false);
        Task<EventoDTO[]> GetAllEventosByTemaAsync(int idUser, string tema, bool includePalestrantes = false);
        Task<EventoDTO> GetAllEventoByIdAsync(int idUser, int idEvento, bool includePalestrantes = false);
    }
}
