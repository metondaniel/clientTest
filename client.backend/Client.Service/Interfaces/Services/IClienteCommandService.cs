using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Service.Commands;
using Product.Service.Entities;
using Product.Service.Interfaces.Repositories;

namespace Product.Service.Interfaces.Services
{
    public interface IClienteCommandService
    {
        Task<Guid> CreateClienteAsync(CreateClienteCommand command);
        Task UpdateClienteAsync(UpdateClienteCommand command);
        Task DeleteClienteAsync(Guid id);
    }

}
