using System.Threading.Tasks;

namespace ProEventos.Persistence
{
    // Criar as chamadas da persistência
    // Classe generica para o insert, update e delete
    public interface IGeralPersist
    {
        // Geral
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        void DeleteRange<T>(T[] entity) where T: class;
        Task<bool> SaveChangesAsync();
    }
}