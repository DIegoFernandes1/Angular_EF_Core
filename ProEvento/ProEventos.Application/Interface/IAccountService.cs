using Microsoft.AspNetCore.Identity;
using ProEventos.Application.DTOs;
using System.Threading.Tasks;

namespace ProEventos.Application.Interface
{
    public interface IAccountService
    {
        Task<bool> UserExists(string username);
        Task<UserUpdateDTO> GetUserByUsernameAsync(string username);
        Task<SignInResult> CheckUserPassWordAsync(UserUpdateDTO userUpdateDTO, string password);
        Task<UserDTO> CreateAccountAsync(UserDTO userDTO);
        Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO);
    }
}
