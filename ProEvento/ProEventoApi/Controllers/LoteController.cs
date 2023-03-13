using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interface;
using System.Threading.Tasks;
using System;
using ProEventos.Application.DTOs;

namespace ProEventos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoteController : ControllerBase
    {
        private readonly ILoteService loteService;

        public LoteController(ILoteService loteService)
        {
            this.loteService = loteService;
        }

        [HttpGet("{idEvento}")]
        public async Task<IActionResult> Get(int idEvento)
        {
            try
            {
                var lotes = await loteService.GetLotesByIdEventoAsync(idEvento);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }

        [HttpPut("{idEvento}")]
        public async Task<IActionResult> SaveLotes(int idEvento, LoteDTO[] loteDTOs)
        {
            try
            {
                var lotes = await loteService.SaveLotes(idEvento, loteDTOs);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{idEvento}/{idLote}")]
        public async Task<IActionResult> Delete(int idEvento, int idLote)
        {
            try
            {
                var lote = await loteService.GetLoteByIdsAsync(idEvento, idLote);
                if (lote == null) return NoContent();

                return await loteService.DeleteLote((int)lote.IdEvento ,lote.Id)
                    ? Ok(new { deletado = true })
                    : BadRequest("lote não deletado");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar deletar lote. Erro: {ex.Message}");
            }
        }
    }
}
