using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Helpers;
using ProEventos.Application.Interface;
using ProEventos.Persistence.Models;
using System;
using System.Threading.Tasks;

namespace ProEvento.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService eventoService;
        private readonly IAccountService accountService;
        private readonly IUtil util;

        private readonly string destino = "Images";

        public EventoController(IEventoService eventoService, IAccountService accountService, IUtil util)
        {
            this.eventoService = eventoService;
            this.accountService = accountService;
            this.util = util;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            try
            {
                var eventos = await eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
                if (eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPage);

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
                var evento = await eventoService.GetAllEventoByIdAsync(User.GetUserId(), id,true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDTO model)
        {
            try
            {
                var evento = await eventoService.AddEvento(User.GetUserId(), model);
                if (evento == null) return BadRequest("Erro ao tentar adicionar evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar adicionar evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("{idEvento}")]
        public async Task<IActionResult> Put(int idEvento, EventoDTO model)
        {
            try
            {
                var evento = await eventoService.UpdateEvento(User.GetUserId(), idEvento, model);
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
                var evento = await eventoService.GetAllEventoByIdAsync(User.GetUserId(), idEvento, true);
                if (evento == null) return NoContent();

                if (await eventoService.DeleteEvento(User.GetUserId(), idEvento))
                {
                    util.DeleteImage(evento.ImagemURL, destino);
                    return Ok(new { deletado = true });
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status400BadRequest, $"Não foi possível deletar o evento.");
                }
                
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar deletar evento. Erro: {ex.Message}");
            }
        }

        [HttpPost("UploadImage/{idEvento}")]
        public async Task<IActionResult> UploadImage(int idEvento)
        {
            try
            {
                var evento = await eventoService.GetAllEventoByIdAsync(User.GetUserId(), idEvento, true);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    util.DeleteImage(evento.ImagemURL, destino);
                    evento.ImagemURL = await util.SaveImage(file, destino);
                }

                var eventoRetorno = await eventoService.UpdateEvento(User.GetUserId(), idEvento,evento);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar alterar imagem. Erro: {ex.Message}");
            }
        }
    }
}
