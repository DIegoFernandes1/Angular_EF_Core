using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Helpers;
using ProEventos.Application.Interface;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Concreta
{
    public class LoteService : ILoteService
    {
        private readonly IGeralPersistence geralPersistence;
        private readonly ILotePersistence lotePersistence;
        private readonly IMapper mapper;

        public LoteService(IGeralPersistence geralPersistence, ILotePersistence lotePersistence, IMapper mapper)
        {
            this.geralPersistence = geralPersistence;
            this.lotePersistence = lotePersistence;
            this.mapper = mapper;
        }

        public async Task<LoteDTO> AddLote(int idEvento, LoteDTO loteDTO)
        {
            try
            {
                var lote = mapper.Map<Lote>(loteDTO);
                lote.IdEvento = idEvento;

                geralPersistence.Add(lote);
                await geralPersistence.SaveChangesAsync();

                return mapper.Map<LoteDTO>(lote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO> UpdateLote(int idEvento, LoteDTO loteDTO)
        {
            try
            {
                var lotes = await lotePersistence.GetLotesByIdEventoAsync(idEvento);
                DomainException.When(lotes == null, "Nenhum lote encontrando para o ID Evento informado.");

                var lote = lotes.FirstOrDefault(x => x.Id == loteDTO.Id);
                loteDTO.IdEvento = idEvento;

                mapper.Map(loteDTO, lote);
                geralPersistence.Update(lote);
                await geralPersistence.SaveChangesAsync();

                return mapper.Map<LoteDTO>(lote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO[]> SaveLotes(int idEvento, LoteDTO[] lotesDTO)
        {
            try
            {
                foreach (var loteDTO in lotesDTO)
                {
                    if(loteDTO.Id == 0)
                    {
                        await AddLote(idEvento, loteDTO);
                    }
                    else
                    {
                        await UpdateLote(idEvento,loteDTO);        
                    }
                }

                var result = await lotePersistence.GetLotesByIdEventoAsync(idEvento);

                return mapper.Map<LoteDTO[]>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int idEvento, int idLote)
        {
            try
            {
                var lote = await lotePersistence.GetLoteByIdsAsync(idEvento, idLote);
                DomainException.When(lote == null, "Lote para delete não encontrado.");

                geralPersistence.Delete(lote);
                return await geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO> GetLoteByIdsAsync(int idEvento, int id)
        {
            try
            {
                var lote = await lotePersistence.GetLoteByIdsAsync(idEvento, id);
                if (lote == null) return null;

                var loteDTO = mapper.Map<LoteDTO>(lote);

                return loteDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO[]> GetLotesByIdEventoAsync(int idEvento)
        {
            try
            {
                var lotes = await lotePersistence.GetLotesByIdEventoAsync(idEvento);
                if (lotes == null) return null;

                var loteDTO = mapper.Map<LoteDTO[]>(lotes);

                return loteDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
