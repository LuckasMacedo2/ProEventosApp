using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence
{
    // Criar as chamadas da persistência
    // Classe generica para o insert, update e delete
    public interface ILotePersist
    {
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        Task<Lote> GetLoteByIdsAsync(int eventoId, int id);
    }
}