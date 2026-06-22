using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class EmprestimoDto
    {
        [Required(ErrorMessage = "O campo CpfMembro é obrigatório.")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve estar no formato válido (000.000.000-00).")]
        public required string CpfMembro { get; set; }

        [Required(ErrorMessage = "O campo CodigoExemplar é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O código do exemplar deve ter entre 3 e 50 caracteres.")]
        public required string CodigoExemplar { get; set; }

        [Required(ErrorMessage = "O campo DiasDeEmprestimo é obrigatório.")]
        [Range(1, 90, ErrorMessage = "O prazo deve ser entre 1 e 90 dias.")]
        public int DiasDeEmprestimo { get; set; } 
    }
}