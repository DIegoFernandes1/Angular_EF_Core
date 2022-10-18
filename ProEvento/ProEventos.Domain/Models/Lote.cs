using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEventos.Domain.Models
{
    public  class Lote
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Quantidade { get; set; }
        public int IdEvento { get; set; }
        public Evento Evento { get; set; }

    }
}
