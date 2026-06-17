namespace BiblioSystem.Dtos.Responses
{
    public class EmprestimoResponseDto
    {
        public int Id { get; set; }

        public DateOnly DataEmprestimo { get; set; }

        public DateOnly DataPrevistaDevolucao { get; set; }

        public DateOnly? DataDevolucao { get; set; }

        public decimal Multa { get; set; }

        public MembroResponseDto? Membro { get; set; }

        public ExemplarResponseDto? Exemplar { get; set; }
    }
}