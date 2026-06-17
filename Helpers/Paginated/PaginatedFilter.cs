using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Helpers.Paginated
{
    public class PaginatedFilter :
        IPaginatedFilter
    {
        public string? Search { get; set; }

        [DefaultValue(1)]
        public int Page { get; set; } = 1;

        [DefaultValue(5)]
        [Range(1, 20)]
        public int Limit { get; set; } = 5;
    }
}