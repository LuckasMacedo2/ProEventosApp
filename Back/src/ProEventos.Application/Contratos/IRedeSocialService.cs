using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models);
        Task<bool> DeleteRedeSocialByEvento(int eventoId, int redeSocialId);
        Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] models);
        Task<bool> DeleteRedeSocialByPalestrante(int palestranteId, int redeSocialId);
        Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int eventoId);
        Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int eventoId, int redeSocialId);
         
    }
}