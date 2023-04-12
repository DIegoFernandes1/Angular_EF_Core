using Microsoft.EntityFrameworkCore;
using ProEventos.Persistence.Data;
using ProEventos.Persistence.Interface;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Concreta
{
    public class GeralPersistence : IGeralPersistence
    {
        private readonly ProEventosContext Context;

        public GeralPersistence(ProEventosContext context)
        {
            Context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            Context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            Context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            Context.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await Context.SaveChangesAsync()) > 0; 
        }
    }
}
