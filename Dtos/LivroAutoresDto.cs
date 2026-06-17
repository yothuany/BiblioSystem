using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Dtos
{
    public class LivroAutoresDto
    {
        [Required]
        public required List<int> Ids
        {
            get;
            set;
        }
    }
}