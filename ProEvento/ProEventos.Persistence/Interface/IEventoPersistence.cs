using ProEventos.Domain.Models;
using ProEventos.Persistence.Models;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Interface
{
    public interface IEventoPersistence : IGeralPersistence
    {
        //EVENTOS
        Task<PageList<Evento>> GetAllEventosAsync(int idUser, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetAllEventoByIdAsync(int idUser, int idEvento, bool includePalestrantes = false);
    }
}
