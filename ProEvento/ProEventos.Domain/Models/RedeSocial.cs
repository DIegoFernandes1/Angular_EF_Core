using System.ComponentModel.DataAnnotations.Schema;

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
