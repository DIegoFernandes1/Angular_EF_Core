using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Domain.Models
{
    public class Palestrante
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public IEnumerable<RedeSocial> RedeSocials { get; set; }
        public IEnumerable<PalestranteEvento> PalestrantesEventos { get; set; }
    }
}
