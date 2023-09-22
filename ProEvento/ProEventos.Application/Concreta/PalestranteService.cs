using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Models;
using System.Threading.Tasks;

namespace ProEventos.Application.Concreta
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantesPersistence palestrantesPersistence;
        private readonly IMapper mapper;

        public PalestranteService(IPalestrantesPersistence palestrantesPersistence, IMapper mapper)
        {
            this.palestrantesPersistence = palestrantesPersistence;
            this.mapper = mapper;
        }

        public async Task<PalestranteDTO> AddPalestrante(int userId, PalestranteAddDTO model)
        {
            var palestrante = mapper.Map<Palestrante>(model);
            palestrante.UserId = userId;

            palestrantesPersistence.Add(palestrante);

            if(await palestrantesPersistence.SaveChangesAsync())
            {
                var palestranreRetorno = await palestrantesPersistence.GetAllPalestranteByUserIdAsync(userId, false);

                return mapper.Map<PalestranteDTO>(palestranreRetorno);
            }

            return null;
        }

        public async Task<PalestranteDTO> UpdatePalestrante(int userId, PalestranteUpdateDTO model)
        {
            var palestrante = await palestrantesPersistence.GetAllPalestranteByUserIdAsync(userId, false);
            if(palestrante == null) return null;

            model.Id = palestrante.Id;
            model.UserId = userId;

            mapper.Map(model, palestrante);

            palestrantesPersistence.Update(palestrante);

            if (await palestrantesPersistence.SaveChangesAsync())
            {
                var palestranreRetorno = await palestrantesPersistence.GetAllPalestranteByUserIdAsync(userId, false);

                return mapper.Map<PalestranteDTO>(palestranreRetorno);
            }

            return null;
        }

        public async Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(int userId, PageParams pageParams, bool includeEventos = false)
        {
            var palestrantes = await palestrantesPersistence.GetAllPalestrantesAsync(pageParams, includeEventos);
            if (palestrantes == null) return null;

            var resultado = mapper.Map<PageList<PalestranteDTO>>(palestrantes);

            resultado.CurrentPage = palestrantes.CurrentPage;
            resultado.TotalPage = palestrantes.TotalPage;
            resultado.PageSize = palestrantes.PageSize;
            resultado.TotalCount = palestrantes.TotalCount;

            return resultado;
        }

        public async Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            var palestrante = await palestrantesPersistence.GetAllPalestranteByUserIdAsync(userId, includeEventos);
            if(palestrante == null) return null;

            var resultado = mapper.Map<PalestranteDTO>(palestrante);

            return resultado;
        }
    }
}