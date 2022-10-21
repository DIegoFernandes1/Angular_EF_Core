using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Domain.Models
{
    public class RedeSocial
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string URL { get; set; }
        [ForeignKey("Evento")]
        public int? IdEvento { get; set; }
        public Evento Evento { get; set; }
        [ForeignKey("Palestrante")]
        public int? IdPalestrante { get; set; }
        public Palestrante Palestrante { get; set; }
    }
}
