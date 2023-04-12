using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Concreta
{
    public class UserPersistence : GeralPersistence, IUserPersistence
    {
        private readonly ProEventosContext proEventosContext;

        public UserPersistence(ProEventosContext proEventosContext) : base(proEventosContext)
        {
            this.proEventosContext = proEventosContext;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await proEventosContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await proEventosContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await proEventosContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}
