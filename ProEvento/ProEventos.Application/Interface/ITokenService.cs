using ProEventos.Application.DTOs;
using System.Threading.Tasks;

namespace ProEventos.Application.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserUpdateDTO userUpdateDTO);
    }
}
