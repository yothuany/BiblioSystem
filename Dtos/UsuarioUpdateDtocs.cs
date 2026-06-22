using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class UsuarioUpdateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public required string Email { get; set; }

        public string? Senha { get; set; }

        [Required(ErrorMessage = "O perfil de acesso é obrigatório.")]
        [RegularExpression("^(Administrador|Bibliotecario)$", ErrorMessage = "O perfil deve ser 'Administrador' ou 'Bibliotecario'.")]
        public required string Perfil { get; set; }
    }
}