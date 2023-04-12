using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProEventos.Domain.Models
{
    public  class Lote
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Quantidade { get; set; }
        [ForeignKey("Evento")]
        public int? IdEvento { get; set; }
        public Evento Evento { get; set; }
    }
}
