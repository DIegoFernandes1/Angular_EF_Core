using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Interface
{
    public interface IEventoPersistence : IGeralPersistence
    {
        //EVENTOS
        Task<Evento[]> GetAllEventosByTemaAsync(int idUser, string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(int idUser, bool includePalestrantes = false);
        Task<Evento> GetAllEventoByIdAsync(int idUser, int idEvento, bool includePalestrantes = false);
    }
}
