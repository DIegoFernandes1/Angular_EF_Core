using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Persistence.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProEvento.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService eventoService;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IAccountService accountService;

        public EventoController(IEventoService eventoService, IWebHostEnvironment hostEnvironment, IAccountService accountService)
        {
            this.eventoService = eventoService;
            this.hostEnvironment = hostEnvironment;
            this.accountService = accountService;
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
                    DeleteImage(evento.ImagemURL);
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
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);
                }

                var eventoRetorno = await eventoService.UpdateEvento(User.GetUserId(), idEvento,evento);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar adicionar evento. Erro: {ex.Message}");
            }
        }

        [NonAction]
        public void DeleteImage(string imagemURL)
        {
            if(imagemURL != null)
            {
                var imagePath = Path.Combine(hostEnvironment.ContentRootPath, "Resources/Images", imagemURL);

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
                .Take(10)
                .ToArray()
                ).Replace(" ","-");

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(hostEnvironment.ContentRootPath, @"Resources/Images", imageName);

            using(var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
