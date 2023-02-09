using ProEventos.Application.DTOs;
using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Application.Interface
{
    public interface IEventoService
    {
        Task<EventoDTO> AddEvento(EventoDTO model);
        Task<EventoDTO> UpdateEvento(int idEvento, EventoDTO model);
        Task<bool> DeleteEvento(int idEvento);

        Task<EventoDTO[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<EventoDTO[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<EventoDTO> GetAllEventoByIdAsync(int idEvento, bool includePalestrantes = false);
    }
}
