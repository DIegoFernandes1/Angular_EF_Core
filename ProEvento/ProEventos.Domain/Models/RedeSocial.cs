using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Domain.Models
{
    public class RedeSocial
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string URL { get; set; }
        public int? EventoId { get; set; }
        public Evento Evento { get; set; }
        public int? IdPalestrante { get; set; }
        public Palestrante Palestrante { get; set; }
    }
}
