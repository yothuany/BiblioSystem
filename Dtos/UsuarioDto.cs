using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public required string Senha { get; set; }

        [Required(ErrorMessage = "O perfil de acesso é obrigatório.")]
        [RegularExpression("^(Administrador|Funcionario)$", ErrorMessage = "O perfil deve ser 'Administrador' ou 'Funcionario'.")]
        public required string Perfil { get; set; }
    }
}