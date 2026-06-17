namespace BiblioSystem.Dtos.Responses
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public required string Email { get; set; }
    }
}