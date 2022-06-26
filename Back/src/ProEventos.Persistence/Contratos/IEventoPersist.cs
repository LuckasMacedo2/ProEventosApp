using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence
{
    // Criar as chamadas da persistência
    // Classe generica para o insert, update e delete
    public interface IEventoPersist
    {
        // Eventos
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false);
    }
}