using Microsoft.AspNetCore.Mvc;
using ProEventos.Domain;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using ProEventos.API.Extensions;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;

        public RedesSociaisController(IRedeSocialService redeSocialService,
                                      IEventoService eventoService,
                                      IPalestranteService palestranteService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            try
            {
                if(!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var redesocial = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
                if(redesocial == null) return NoContent();

                return Ok(redesocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar as redes sociais do evento. Erro: {ex.Message}");
                
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();
                
                var redesocial = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                if(redesocial == null) return NoContent();

                return Ok(redesocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar as redes sociais do palestrante. Erro: {ex.Message}");
                
            }
        }

        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if(!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var RedeSocials = await _redeSocialService.SaveByEvento(eventoId, models);
                if(RedeSocials == null) return NoContent();
                return Ok(RedeSocials);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar as redes sociais do evento. Erro: {ex.Message}");
                
            }
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();

                var RedeSocials = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);
                if(RedeSocials == null) return NoContent();
                return Ok(RedeSocials);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar as redes sociais do palestrante. Erro: {ex.Message}");
                
            }
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if(!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var RedeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);

                if(RedeSocial == null) return NoContent();

                return await _redeSocialService.DeleteRedeSocialByEvento(eventoId, redeSocialId) ?
                       Ok(new { message = "Deletada"})
                       : throw new Exception("Ocorreu um erro ao deletar a rede social");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rede social. Erro: {ex.Message}");
            }
        }

        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if(palestrante == null) return Unauthorized();

                var RedeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId);
                if(RedeSocial == null) return NoContent();

                return await _redeSocialService.DeleteRedeSocialByPalestrante(palestrante.Id, redeSocialId) ?
                       Ok(new { message = "Deletada"})
                       : throw new Exception("Ocorreu um erro ao deletar a rede social");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar rede social. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if(evento == null) return false;
            return true;
        }


    }
}
