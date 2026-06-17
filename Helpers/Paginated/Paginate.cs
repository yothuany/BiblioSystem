using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BiblioSystem.Helpers.Paginated
{
    public class Paginate<T>
    {
        public async static Task<
            PaginatedResponse<TDto>
        > Set<TDto>(
            IQueryable<T> query,
            IPaginatedFilter paginate,
            IMapper mapper
        )
        {
            var totalItems =
                await query.CountAsync();

            var totalPages =
                (int)Math.Ceiling(
                    totalItems /
                    (double)paginate.Limit
                );

            var list =
                await query
                .Skip(
                    (paginate.Page - 1)
                    * paginate.Limit
                )
                .Take(
                    paginate.Limit
                )
                .ToListAsync();

            return new PaginatedResponse<TDto>()
            {
                Page =
                    paginate.Page,

                Limit =
                    paginate.Limit,

                TotalItems =
                    totalItems,

                TotalPages =
                    totalPages,

                Data =
                    mapper
                    .Map<
                        IEnumerable<TDto>
                    >(list)
            };
        }
    }
}