using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Product.Service.Dtos;

namespace Product.Service.Queries
{
    public record GetClienteByIdQuery(Guid Id) : IRequest<ClienteDto>;
}
