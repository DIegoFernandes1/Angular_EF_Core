using System;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Api.Models
{
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }
        public string Local { get; set; }
        public DateTime DataEvento { get; set; }
        public string Tema { get; set; }
        public int QuantidadePessoas { get; set; }
        public string Lote { get; set; }
        public string ImagemURL { get; set; }
    }
}
