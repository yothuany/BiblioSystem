using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class EmprestimoDto
    {
        [Required]
        public int MembroId { get; set; }

        [Required]
        public int ExemplarId { get; set; }

        [Required]
        public DateOnly DataPrevistaDevolucao { get; set; }
    }
}