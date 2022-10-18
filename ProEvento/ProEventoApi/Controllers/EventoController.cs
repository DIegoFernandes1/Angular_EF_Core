using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Services;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProEvento.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private EventoService _eventoService;

        public EventoController(EventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _eventoService.Get();
        }

        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return _eventoService.Get(id);
        }

    }
}
