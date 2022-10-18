using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Interface
{
    public interface IPalestrantesPersistence
    {
        //PALESTRANTES
        Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos);
        Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos);
        Task<Palestrante> GetAllPalestranteByIdAsync(int idPalestrante, bool includeEventos);
    }
}
