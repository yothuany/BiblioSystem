using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BiblioSystem.Helpers.Paginated
{
    public class PaginatedFilter :
        IPaginatedFilter
    {
        public string? Search { get; set; }

        [SwaggerParameter("O número da página que você deseja visualizar (Começa em 1).", Required = false)]
        public int Page { get; set; } = 1;

        [SwaggerParameter("Quantidade de registros exibidos por página.", Required = false)]
        [Range(1, 100)]
        public int Limit { get; set; } = 25;
    }
}