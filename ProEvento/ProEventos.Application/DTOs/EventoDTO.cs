using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.DTOs
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
         StringLength(50, MinimumLength = 3, ErrorMessage = "Intervalo permitido é de 3 a 50 caracteres")]
        public string Tema { get; set; }

        [Display(Name = "quantidade pessoas"),
         Range(1, 120000, ErrorMessage = "{0} não pode ser menor que 1 e maior que 120.000")]
        public int QuantidadePessoas { get; set; }

        [RegularExpression (@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "não é uma imagem válida. (gif, jpg, jpeg, bmp ou png)")]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"),
         Phone(ErrorMessage = "o campo {0} é inválido")]
        public string Telefone { get; set; }

        [Display (Name = "e-mail"),
         Required(ErrorMessage = "O campo {0} é obrigatório"),
         EmailAddress(ErrorMessage = "É necessário ser um {0} válido")]
        public string Email { get; set; }

        public int UserId { get; set; }
        public UserDTO UserDTO { get; set; }

        public IEnumerable<LoteDTO> Lote { get; set; }
        public IEnumerable<RedeSocialDTO> RedeSocial { get; set; }
        public IEnumerable<PalestranteDTO> Palestrantes { get; set; }
    }
}
