using BiblioSystem.Helpers.Paginated;

namespace BiblioSystem.Controllers.Filters
{
    public class LivroFilter
        : PaginatedFilter
    {
        public string? Editora
        {
            get; set;
        }

        public int? CategoriaId
        {
            get; set;
        }

        public string? Autor
        {
            get; set;
        }
    }
}