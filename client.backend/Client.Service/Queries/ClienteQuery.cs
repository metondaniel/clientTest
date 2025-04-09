using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Product.Service.Dtos;

namespace Product.Service.Queries
{
    public record ClienteQuery(
    ClienteFilter? Filter = null,
    int PageNumber = 1,
    string SortBy = "NomeRazaoSocial",
    bool SortDescending = false,
    int PageSize = 20) : IRequest<PagedResult<ClienteDto>>;

    public record PagedResult<T>(
        List<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize);
}
