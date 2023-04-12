using ProEventos.Application.DTOs;
using System.Threading.Tasks;

namespace ProEventos.Application.Interface
{
    public interface ILoteService
    {
        Task<LoteDTO[]> SaveLotes(int idEvento, LoteDTO[] lotesDTO);
        Task<LoteDTO> AddLote(int idEvento, LoteDTO loteDTO);
        Task<LoteDTO> UpdateLote(int idEvento, LoteDTO loteDTO);
        Task<bool> DeleteLote(int idEvento, int idLote);
        Task<LoteDTO[]> GetLotesByIdEventoAsync(int idEvento);
        Task<LoteDTO> GetLoteByIdsAsync(int idEvento, int id);
    }
}
