using ProEventos.Domain.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Interface
{
    public interface IUserPersistence : IGeralPersistence
    {
        Task<IEnumerable<User>> GetUsersAsync(); 
        Task<User> GetUserByIdAsync(int id); 
        Task<User> GetUserByUsernameAsync(string username); 
    }
}
