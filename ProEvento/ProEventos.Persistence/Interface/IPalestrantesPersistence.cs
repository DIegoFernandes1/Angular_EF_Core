using ProEventos.Domain.Models;
using ProEventos.Persistence.Models;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Interface
{
    public interface IPalestrantesPersistence : IGeralPersistence
    {
        //PALESTRANTES
        Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
        Task<Palestrante> GetAllPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}
