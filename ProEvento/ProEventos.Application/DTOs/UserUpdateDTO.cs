namespace ProEventos.Application.DTOs
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Username { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Funcao { get; set; }
        public string Decricao { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
