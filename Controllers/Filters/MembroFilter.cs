using BiblioSystem.Helpers.Paginated;

namespace BiblioSystem.Controllers.Filters
{
    public class MembroFilter
        : PaginatedFilter
    {
        public string? CPF { get; set; }

        public string? Email { get; set; }
    }
}