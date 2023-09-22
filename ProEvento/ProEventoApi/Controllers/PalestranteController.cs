using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Persistence.Models;
using System;
using System.Threading.Tasks;

namespace ProEventos.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        private readonly IPalestranteService palestranteService;

        public PalestranteController(IPalestranteService palestranteService)
        {
            this.palestranteService = palestranteService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PageParams pageParams)
        {
            try
            {
                var palestrantes = await palestranteService.GetAllPalestrantesAsync(User.GetUserId(), pageParams, true);
                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPage);

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetPalestrante()
        {
            try
            {
                var palestrantes = await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
                if (palestrantes == null) return NoContent();

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPost()]
        public async Task<IActionResult> post(PalestranteAddDTO palestranteAddDTO)
        {
            try
            {
                var palestrante = await palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);

                if (palestrante == null)
                    palestrante = await palestranteService.AddPalestrante(User.GetUserId(), palestranteAddDTO);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar cadastrar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPut()]
        public async Task<IActionResult> put(PalestranteUpdateDTO palestranteUpdateDTO)
        {
            try
            {
                var palestrante = await palestranteService.UpdatePalestrante(User.GetUserId(), palestranteUpdateDTO);
                if (palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar palestrante. Erro: {ex.Message}");
            }
        }
    }
}