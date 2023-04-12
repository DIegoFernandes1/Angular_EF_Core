using ProEventos.Domain.Models;
using System.Threading.Tasks;

namespace ProEventos.Persistence.Interface
{
    public interface ILotePersistence : IGeralPersistence
    {
        /// <summary>
        /// metodo get que retornará uma lista de lotes por id evento
        /// </summary>
        /// <param name="idEvento">codigo chave da tabela evento</param>
        /// <returns>lista de lotes</returns>
        Task<Lote[]> GetLotesByIdEventoAsync(int idEvento);

        /// <summary>
        /// metodo get que retornará apenas um lote
        /// </summary>
        /// <param name="idEvento">codigo chave da tabela evento</param>
        /// <param name="id">codigo chave da tabela lote</param>
        /// <returns>apenas 1 lote</returns>
        Task<Lote> GetLoteByIdsAsync(int idEvento, int id);
    }
}
