using System;

namespace BiblioSystem.Dtos.Responses
{
    public class EmprestimoResponseDto
    {
        public int Id { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public decimal ValorMulta { get; set; }
        public required string Status { get; set; }
        public int MembroId { get; set; }
        public int ExemplarId { get; set; }
        public string? CodigoExemplar { get; set; } 
        public required string NomeMembro { get; set; }
        public required string TituloLivro { get; set; }
    }
}