using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using System.Threading.Tasks;
using System;
using ProEventos.Domain.Models;

namespace ProEventos.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RedeSocialController : ControllerBase
    {
        private readonly IRedeSocialService redeSocialService;
        private readonly IEventoService eventoService;
        private readonly IPalestranteService palestranteService;

        public RedeSocialController(IRedeSocialService redeSocialService, IEventoService eventoService, IPalestranteService palestranteService)
        {
            this.redeSocialService = redeSocialService;
            this.eventoService = eventoService;
            this.palestranteService = palestranteService;
        }

        [HttpGet("evento/{idEvento}")]
        public async Task<IActionResult> GetByEvento(int idEvento)
        {
            try
            {
                if(!await AutorEvento(idEvento))
                    return Unauthorized();

                var redesociais = await redeSocialService.GetAllByEventoIdAsync(idEvento);

                if (redesociais == null) 
                    return NoContent();


                return Ok(redesociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar rede social por evento. Erro: {ex.Message}");
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var palestrante = await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrante == null) 
                    return Unauthorized();

                var redeSociais = await redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);

                if(redeSociais == null)
                    return NoContent();

                return Ok(redeSociais);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar rede social por palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPost("evento/{idEvento}")]
        public async Task<IActionResult> SaveByEvento(int idEvento, RedeSocialDTO[] redeSocialDTO)
        {
            try
            {
                if (!await AutorEvento(idEvento))
                    return Unauthorized();

                var redeSocial = await redeSocialService.SaveByEvento(idEvento, redeSocialDTO);

                if (redeSocial == null)
                    return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar cadastrar rede social por evento. Erro: {ex.Message}");
            }
        }

        [HttpPost("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDTO[] redeSocialDTO)
        {
            try
            {
                var palestrante = await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrante == null)
                    return Unauthorized();

                var redeSocial = await redeSocialService.SaveByPalestrante(palestrante.Id, redeSocialDTO);

                if (redeSocial == null)
                    return NoContent();

                return Ok(redeSocial);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar cadastrar rede social por palestrante. Erro: {ex.Message}");
            }
        }

        [HttpDelete("evento/{idEvento}/{idRedeSocial}")]
        public async Task<IActionResult> DeleteByEvento(int idEvento, int idRedeSocial)
        {
            try
            {
                if (!await AutorEvento(idEvento))
                    return Unauthorized();

                var redeSocial = await redeSocialService.GetRedeSocialEventoByIdsAsync(idEvento, idRedeSocial);

                if (redeSocial == null)
                    return NoContent();

               return await redeSocialService.DeleteByEvento(idEvento, redeSocial.Id) 
                    ? Ok(new { message = "Rede social deletada"}) 
                    : throw new Exception("Ocorreu um problema não especifico ao tentar deletar rede social por evento.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar rede social por evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("palestrante/{idRedeSocial}")]
        public async Task<IActionResult> DeleteByPalestrante(int idRedeSocial)
        {
            try
            {
                var palestrante = await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrante == null)
                    return Unauthorized();

                var redeSocial = await redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, idRedeSocial);

                if (redeSocial == null)
                    return NoContent();

                return await redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocial.Id)
                     ? Ok(new { message = "Rede social deletada" })
                     : throw new Exception("Ocorreu um problema não especifico ao tentar deletar rede social por palestrante.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar rede social por palestrante. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int idEvento)
        {
            var evento = await eventoService.GetAllEventoByIdAsync(User.GetUserId(), idEvento, false);

            if (evento == null) 
                return false;

            return true;
        }
    }
}