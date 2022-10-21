using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProEventos.Domain.Models
{
    public class PalestranteEvento
    {
        public int Id { get; set; }
        [ForeignKey("Palestrante")]
        public int IdPalestrante { get; set; }
        public Palestrante Palestrante { get; set; }
        [ForeignKey("Evento")]
        public int IdEvento { get; set; }
        public Evento Evento { get; set; }
    }
}
