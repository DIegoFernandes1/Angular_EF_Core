using AutoMapper;
using Microsoft.Extensions.Logging;
using ProEventos.Application.DTOs;
using ProEventos.Application.Helpers;
using ProEventos.Application.Interface;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Concreta;
using ProEventos.Persistence.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Concreta
{
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IRedeSocialPersistence redeSocialPersistence;
        private readonly IMapper mapper;

        public RedeSocialService(IRedeSocialPersistence redeSocialPersistence, IMapper mapper)
        {
            this.redeSocialPersistence = redeSocialPersistence;
            this.mapper = mapper;
        }

        private async Task AddRedeSocial(int id, RedeSocialDTO model, bool isEvento)
        {
            var redeSocial = mapper.Map<RedeSocial>(model);

            if (isEvento)
            {
                redeSocial.IdEvento = id;
                redeSocial.IdPalestrante = null;
            }
            else
            {
                redeSocial.IdPalestrante = id;
                redeSocial.IdEvento = null;
            }

            redeSocialPersistence.Add(redeSocial);
            await redeSocialPersistence.SaveChangesAsync();
        }

        public async Task<RedeSocialDTO[]> SaveByEvento(int eventoId, RedeSocialDTO[] models)
        {
            var redeSociais = await redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
            if (redeSociais == null) return null;

            foreach (var model in models)
            {
                if (model.Id == 0)
                {
                    await AddRedeSocial(eventoId, model, true);
                }
                else
                {
                    var redeSocial = redeSociais.FirstOrDefault(rs => rs.Id == model.Id);
                    model.IdEvento = eventoId;

                    mapper.Map(model, redeSocial);

                    redeSocialPersistence.Update(redeSocial);

                    await redeSocialPersistence.SaveChangesAsync();
                }
            }
            var result = await redeSocialPersistence.GetAllByEventoIdAsync(eventoId);

            return mapper.Map<RedeSocialDTO[]>(result);
        }

        public async Task<RedeSocialDTO[]> SaveByPalestrante(int palestranteId, RedeSocialDTO[] models)
        {
            var redeSociais = await redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
            if (redeSociais == null) return null;

            foreach (var model in models)
            {
                if (model.Id == 0)
                {
                    await AddRedeSocial(palestranteId, model, false);
                }
                else
                {
                    var redeSocial = redeSociais.FirstOrDefault(rs => rs.Id == model.Id);
                    model.IdPalestrante = palestranteId;

                    mapper.Map(model, redeSocial);

                    redeSocialPersistence.Update(redeSocial);

                    await redeSocialPersistence.SaveChangesAsync();
                }
            }
            var result = await redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);

            return mapper.Map<RedeSocialDTO[]>(result);
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            var redeSocial = await redeSocialPersistence.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
            DomainException.When(redeSocial == null, "Rede social por evento para delete não encontrado.");

            redeSocialPersistence.Delete(redeSocial);
            return await redeSocialPersistence.SaveChangesAsync();
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            var redeSocial = await redeSocialPersistence.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
            DomainException.When(redeSocial == null, "Rede social por palestrante para delete não encontrado.");

            redeSocialPersistence.Delete(redeSocial);
            return await redeSocialPersistence.SaveChangesAsync();
        }

        public async Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId)
        {
            var redeSocials = await redeSocialPersistence.GetAllByEventoIdAsync(eventoId);
            if (redeSocials == null) return null;

            return mapper.Map<RedeSocialDTO[]>(redeSocials);
        }

        public async Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            var redeSocials = await redeSocialPersistence.GetAllByPalestranteIdAsync(palestranteId);
            if (redeSocials == null) return null;

            return mapper.Map<RedeSocialDTO[]>(redeSocials);
        }

        public async Task<RedeSocialDTO> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            var redeSocial = await redeSocialPersistence.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
            if (redeSocial == null) return null;

            return mapper.Map<RedeSocialDTO>(redeSocial);
        }

        public async Task<RedeSocialDTO> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            var redeSocial = await redeSocialPersistence.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
            if (redeSocial == null) return null;

            return mapper.Map<RedeSocialDTO>(redeSocial);
        }
    }
}