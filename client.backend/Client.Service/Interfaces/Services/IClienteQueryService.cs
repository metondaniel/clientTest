
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Service.Dtos;
using Product.Service.Entities;
using Product.Service.Interfaces.Repositories;
using Product.Service.Queries;

namespace Product.Service.Interfaces.Services
{
    public interface IClienteQueryService
    {
        Task<ClienteDto> GetClienteByIdAsync(Guid id);
        Task<IEnumerable<ClienteDto>> GetAllClientesAsync();
        Task<IEnumerable<ClienteDto>> GetClientesByFilterAsync(ClienteFilter filter);
    }
}
