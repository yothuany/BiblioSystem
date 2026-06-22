namespace BiblioSystem.Dtos.Responses
{
    public class ReservaResponseDto
    {
        public int Id { get; set; }

        public DateTime DataReserva { get; set; }

        public required string Status { get; set; }

        public int MembroId { get; set; }

        public string? MembroNome { get; set; }

        public int LivroId { get; set; }

        public string? LivroTitulo { get; set; }
    }
}