namespace BiblioSystem.Dtos.Responses
{
    public class MembroResponseDto
    {
        public int Id { get; set; }

        public required string NomeCompleto { get; set; }

        public required string CPF { get; set; }

        public required string Email { get; set; }

        public required string Telefone { get; set; }

        public DateOnly DataCadastro { get; set; }
    }
}