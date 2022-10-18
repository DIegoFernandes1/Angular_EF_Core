using ProEventos.Application.Interface;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Application.Concreta
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersistence EventoPersistence;
        private readonly IGeralPersistence geralPersistence;

        //GERAL
        public EventoService(IEventoPersistence eventoPersistence, IGeralPersistence geralPersistence)
        {
            this.EventoPersistence = eventoPersistence;
            this.geralPersistence = geralPersistence;
        }

        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                geralPersistence.Add<Evento>(model);

                if (await geralPersistence.SaveChangesAsync())
                {
                    return await EventoPersistence.GetAllEventoByIdAsync(model.Id);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<Evento> UpdateEvento(int idEvento, Evento model)
        {
            try
            {
                var evento = await EventoPersistence.GetAllEventoByIdAsync(idEvento);

                if (evento == null) return null;

                model.Id = evento.Id;
                geralPersistence.Update<Evento>(model);

                if (await geralPersistence.SaveChangesAsync())
                {
                    return await EventoPersistence.GetAllEventoByIdAsync(model.Id);
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

                if (evento == null) throw new Exception("Evento não encontrado");

                geralPersistence.Delete<Evento>(evento);
                return await geralPersistence.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //EVENTOS
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await EventoPersistence.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetAllEventoByIdAsync(int idEvento, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await EventoPersistence.GetAllEventoByIdAsync(idEvento, includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await EventoPersistence.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
