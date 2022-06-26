using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence
{
    // Criar as chamadas da persistência
    // Classe generica para o insert, update e delete
    public interface IUserPersist : IGeralPersist
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUserNameAsync(string userName);
    }
}