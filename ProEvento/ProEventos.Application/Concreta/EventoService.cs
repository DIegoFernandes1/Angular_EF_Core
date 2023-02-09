using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Helpers;
using ProEventos.Application.Interface;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interface;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application.Concreta
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersistence EventoPersistence;
        private readonly IGeralPersistence geralPersistence;
        private readonly IMapper mapper;

        //GERAL
        public EventoService(IEventoPersistence eventoPersistence, 
                            IGeralPersistence geralPersistence, 
                            IMapper mapper)
        {
            this.EventoPersistence = eventoPersistence;
            this.geralPersistence = geralPersistence;
            this.mapper = mapper;
        }

        public async Task<EventoDTO> AddEvento(EventoDTO model)
        {
            try
            {
                var evento = mapper.Map<Evento>(model);
                geralPersistence.Add(evento);

                if (await geralPersistence.SaveChangesAsync())
                {
                    var eventoDTO = await EventoPersistence.GetAllEventoByIdAsync(evento.Id);
                    return mapper.Map<EventoDTO>(eventoDTO);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<EventoDTO> UpdateEvento(int idEvento, EventoDTO model)
        {
            try
            {
                var evento = await EventoPersistence.GetAllEventoByIdAsync(idEvento);

                if (evento == null) return null;

                model.Id = evento.Id;

                mapper.Map(model, evento);
                geralPersistence.Update(evento);

                if (await geralPersistence.SaveChangesAsync())
                {
                    var eventoDTO = await EventoPersistence.GetAllEventoByIdAsync(evento.Id);
                    return mapper.Map<EventoDTO>(eventoDTO);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int idEvento)
        {
            try
            {
                var evento = await EventoPersistence.GetAllEventoByIdAsync(idEvento);

                DomainException.When(evento == null, "Evento não encontrado");

                geralPersistence.Delete(evento);
                return await geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //EVENTOS
        public async Task<EventoDTO[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await EventoPersistence.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                var eventosDTO = mapper.Map<EventoDTO[]>(eventos);

                return eventosDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> GetAllEventoByIdAsync(int idEvento, bool includePalestrantes = false)
        {
            try
            {
                var evento = await EventoPersistence.GetAllEventoByIdAsync(idEvento, includePalestrantes);
                if (evento == null) return null;

                var eventoDTO = mapper.Map<EventoDTO>(evento);

                return eventoDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await EventoPersistence.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                var eventosDTO = mapper.Map<EventoDTO[]>(eventos);

                return eventosDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
