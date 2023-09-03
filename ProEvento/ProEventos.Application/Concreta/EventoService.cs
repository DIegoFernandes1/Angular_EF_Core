using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Helpers;
using ProEventos.Application.Interface;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Models;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application.Concreta
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersistence EventoPersistence;
        private readonly IMapper mapper;

        //GERAL
        public EventoService(IEventoPersistence eventoPersistence,
                            IMapper mapper)
        {
            this.EventoPersistence = eventoPersistence;
            this.mapper = mapper;
        }

        public async Task<EventoDTO> AddEvento(int idUser, EventoDTO model)
        {
            try
            {
                var evento = mapper.Map<Evento>(model);

                evento.UserId = idUser;
                EventoPersistence.Add(evento);

                if (await EventoPersistence.SaveChangesAsync())
                {
                    var eventoDTO = await EventoPersistence.GetAllEventoByIdAsync(idUser, evento.Id);
                    return mapper.Map<EventoDTO>(eventoDTO);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<EventoDTO> UpdateEvento(int idUser, int idEvento, EventoDTO model)
        {
            try
            {
                var evento = await EventoPersistence.GetAllEventoByIdAsync(idUser, idEvento);

                if (evento == null) return null;

                model.Id = evento.Id;
                model.UserId = idUser;

                mapper.Map(model, evento);
                EventoPersistence.Update(evento);

                if (await EventoPersistence.SaveChangesAsync())
                {
                    var eventoDTO = await EventoPersistence.GetAllEventoByIdAsync(idUser, evento.Id);
                    return mapper.Map<EventoDTO>(eventoDTO);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int idUser, int idEvento)
        {
            try
            {
                var evento = await EventoPersistence.GetAllEventoByIdAsync(idUser, idEvento);

                DomainException.When(evento == null, "Evento não encontrado");

                EventoPersistence.Delete(evento);
                return await EventoPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //EVENTOS
        public async Task<PageList<EventoDTO>> GetAllEventosAsync(int idUser, PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await EventoPersistence.GetAllEventosAsync(idUser, pageParams, includePalestrantes);

                if (eventos == null) return null;

                var eventosDTO = mapper.Map<PageList<EventoDTO>>(eventos);

                eventosDTO.CurrentPage = eventos.CurrentPage;
                eventosDTO.PageSize = eventos.PageSize;
                eventosDTO.TotalCount = eventos.TotalCount;
                eventosDTO.TotalPage = eventos.TotalPage;

                return eventosDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> GetAllEventoByIdAsync(int idUser, int idEvento, bool includePalestrantes = false)
        {
            try
            {
                var evento = await EventoPersistence.GetAllEventoByIdAsync(idUser, idEvento, includePalestrantes);
                if (evento == null) return null;

                var eventoDTO = mapper.Map<EventoDTO>(evento);

                return eventoDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
