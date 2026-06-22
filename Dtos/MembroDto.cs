using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class MembroDto
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public required string Telefone { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$",
            ErrorMessage = "O formato do CPF é inválido.")]
        public required string Cpf { get; set; }
    }
}