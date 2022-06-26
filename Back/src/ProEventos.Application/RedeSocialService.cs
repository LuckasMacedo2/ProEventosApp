using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialPersist _redeSocialPersist;
        private IMapper _mapper { get; }
        public RedeSocialService(IRedeSocialPersist redeSocialPersist, IMapper mapper)
        {
            _mapper = mapper;
            _redeSocialPersist = redeSocialPersist;
        }
        public async Task AddRedeSocial(int id, RedeSocialDto model, bool isEvento)
        {
            try
            {
                var RedeSocial = _mapper.Map<RedeSocial>(model);

                if(isEvento)
                {
                    RedeSocial.EventoId = id;
                    RedeSocial.PalestranteId = null;
                }
                else
                {
                    RedeSocial.PalestranteId = id;
                    RedeSocial.EventoId = null;
                }

                _redeSocialPersist.Add<RedeSocial>(RedeSocial);
                await _redeSocialPersist.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var RedeSocials = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);
                if (RedeSocials == null) return null;

                foreach(var model in models)
                {
                    if(model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else
                    {
                        var redesocial = RedeSocials.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);   
                        model.EventoId = eventoId;

                        _mapper.Map(model, redesocial);   
                        _redeSocialPersist.Update<RedeSocial>(redesocial);
                        await _redeSocialPersist.SaveChangesAsync();                
                    }    
                }

                var RedeSocialRetorno = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);
                return _mapper.Map<RedeSocialDto[]>(RedeSocialRetorno);
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] models)
        {
            try
            {
                var RedeSocials = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);
                if (RedeSocials == null) return null;

                foreach(var model in models)
                {
                    if(model.Id == 0)
                    {
                        await AddRedeSocial(palestranteId, model, false);
                    }
                    else
                    {
                        var redesocial = RedeSocials.FirstOrDefault(RedeSocial => RedeSocial.Id == model.Id);   
                        model.PalestranteId = palestranteId;

                        _mapper.Map(model, redesocial);   
                        _redeSocialPersist.Update<RedeSocial>(redesocial);
                        await _redeSocialPersist.SaveChangesAsync();                
                    }    
                }

                var RedeSocialRetorno = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);
                return _mapper.Map<RedeSocialDto[]>(RedeSocialRetorno);
                

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRedeSocialByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersist.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (RedeSocial == null) throw new Exception("Rede Social port evento a ser deletada não encontrado");

                _redeSocialPersist.Delete<RedeSocial>(RedeSocial);
                return await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteRedeSocialByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (RedeSocial == null) throw new Exception("Rede Social por palestrante a ser deletada não encontrado");

                _redeSocialPersist.Delete<RedeSocial>(RedeSocial);
                return await _redeSocialPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var RedeSocials = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);
                if (RedeSocials == null) return null;

                var resultados = _mapper.Map<RedeSocialDto[]>(RedeSocials);

                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var RedeSocials = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);
                if (RedeSocials == null) return null;

                var resultados = _mapper.Map<RedeSocialDto[]>(RedeSocials);

                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersist.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (RedeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDto>(RedeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var RedeSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (RedeSocial == null) return null;

                var resultado = _mapper.Map<RedeSocialDto>(RedeSocial);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}