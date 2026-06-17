using BiblioSystem.Helpers.Paginated;

namespace BiblioSystem.Controllers.Filters
{
    public class ReservaFilter
        : PaginatedFilter
    {
        public string? StatusReserva { get; set; }

        public int? LivroId { get; set; }
    }
}