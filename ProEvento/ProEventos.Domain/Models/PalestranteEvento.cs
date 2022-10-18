using System.ComponentModel.DataAnnotations;

namespace ProEventos.Domain.Models
{
    public class PalestranteEvento
    {
        [Key]
        public int IdPalestrante { get; set; }
        public Palestrante Palestrante { get; set; }
        public int IdEvento { get; set; }
        public Evento Evento { get; set; }
    }
}
