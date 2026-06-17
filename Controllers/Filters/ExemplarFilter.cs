using BiblioSystem.Helpers.Paginated;

namespace BiblioSystem.Controllers.Filters
{
    public class ExemplarFilter
        : PaginatedFilter
    {
        public string? Status { get; set; }

        public int? LivroId { get; set; }
    }
}