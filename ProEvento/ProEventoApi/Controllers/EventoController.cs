using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interface;
using ProEventos.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ProEvento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService EventoService;

        public EventoController(IEventoService eventoService)
        {
            EventoService = eventoService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await EventoService.GetAllEventosAsync(true);
                if (eventos == null) return NotFound("Nenhum evento encontrado");

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await EventoService.GetAllEventoByIdAsync(id,true);
                if (evento == null) return NotFound("Nenhum evento por ID encontrado");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var evento = await EventoService.GetAllEventosByTemaAsync(tema, true);
                if (evento == null) return NotFound("Nenhum evento por tema encontrado");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                var evento = await EventoService.AddEvento(model);
                if (evento == null) return BadRequest("Erro ao tentar adicionar evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar adicionar evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("{idEvento}")]
        public async Task<IActionResult> Put(int idEvento, Evento model)
        {
            try
            {
                var evento = await EventoService.UpdateEvento(idEvento, model);
                if (evento == null) return BadRequest("Erro ao tentar atualizar evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar atualizar evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{idEvento}")]
        public async Task<IActionResult> Delete(int idEvento)
        {
            try
            {
                if(await EventoService.DeleteEvento(idEvento))
                {
                    return Ok("Deletado");
                }
                else
                {
                    return BadRequest("Evento não deletado");
                }
                
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar deletar evento. Erro: {ex.Message}");
            }
        }

    }
}
