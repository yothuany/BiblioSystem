using BiblioSystem.Helpers.Paginated;

namespace BiblioSystem.Controllers.Filters
{
    public class LivroFilter : PaginatedFilter
    {
        public string? Autor
        {
            get; set;
        }
    }
}