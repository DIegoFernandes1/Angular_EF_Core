using ProEventos.Application.DTOs;
using ProEventos.Persistence.Models;
using System.Threading.Tasks;

namespace ProEventos.Application.Interface
{
    public interface IEventoService
    {
        Task<EventoDTO> AddEvento(int idUser, EventoDTO model);
        Task<EventoDTO> UpdateEvento(int idUser, int idEvento, EventoDTO model);
        Task<bool> DeleteEvento(int idUser, int idEvento);

        Task<PageList<EventoDTO>> GetAllEventosAsync(int idUser, PageParams pageParams, bool includePalestrantes = false);
        Task<EventoDTO> GetAllEventoByIdAsync(int idUser, int idEvento, bool includePalestrantes = false);
    }
}
