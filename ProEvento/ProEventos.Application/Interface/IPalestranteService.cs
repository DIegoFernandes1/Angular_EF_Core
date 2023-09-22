using ProEventos.Application.DTOs;
using ProEventos.Persistence.Models;
using System.Threading.Tasks;

namespace ProEventos.Application.Interface
{
    public interface IPalestranteService
    {
        Task<PalestranteDTO> AddPalestrante(int userId, PalestranteAddDTO model);   
        Task<PalestranteDTO> UpdatePalestrante(int userId, PalestranteUpdateDTO model);   
        Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(int userId, PageParams pageParams, bool includeEventos = false);   
        Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);   
    }
}