namespace BiblioSystem.Dtos.Responses
{
    public class MembroResponseDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Telefone { get; set; }
        public required string Cpf { get; set; }
    }
}