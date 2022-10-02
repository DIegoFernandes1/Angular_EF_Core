using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Data;
using ProEventos.Api.Models;
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
        private readonly DataContext _context;
        public EventoController(DataContext dataContext) 
        {
            _context = dataContext;
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
           return _context.Evento;
        }

        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return _context.Evento.FirstOrDefault(x => x.IdEvento == id);
        }

    }
}
