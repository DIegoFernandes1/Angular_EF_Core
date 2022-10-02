using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Models;
using System;

namespace ProEvento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {

        [HttpGet]
        public Evento Get()
        {
            return new Evento()
            {
                EventoId = 1,
                Tema = ".NET Core 5 + Angular",
                Local = "Campinas - SP",
                Lote = "1° lote",
                QuantidadePessoas = 500,
                DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy")

            };
        }

    }
}
