using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Product.Service.Dtos;
using Product.Service.Entities;
using Product.Service.Interfaces.Repositories;

namespace Product.Service.Queries
{
    public record GetAllClientesQuery : IRequest<IEnumerable<ClienteDto>>;

    // Application/Queries/Handlers/ClienteQueryHandler.cs
    public class ClienteQueryHandler :
        IRequestHandler<GetClienteByIdQuery, ClienteDto>,
        IRequestHandler<GetAllClientesQuery, IEnumerable<ClienteDto>>
    {
        private readonly IClienteRepository _repository;

        public ClienteQueryHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClienteDto> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _repository.GetByIdAsync(request.Id);
            return MapToDto(cliente);
        }

        public async Task<IEnumerable<ClienteDto>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
        {
            var clientes = await _repository.GetAllAsync();
            return clientes.Select(MapToDto);
        }

        private static ClienteDto MapToDto(Cliente cliente) => new(
            cliente.Id,
            cliente.NomeRazaoSocial,
            cliente.CpfCnpj,
            cliente.Email,
            new EnderecoDto(cliente.Endereco.Bairro, cliente.Endereco.Logradouro, cliente.Endereco.Numero,
                cliente.Endereco.Bairro, cliente.Endereco.Cidade, cliente.Endereco.Estado),
            cliente.Tipo,
            cliente.DataNascimento,
            cliente.Telefone,
            cliente.InscricaoEstadual,
            cliente.IsentoIE);
    }
}
