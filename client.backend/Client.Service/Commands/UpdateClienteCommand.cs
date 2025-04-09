using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Product.Service.Entities;
using Product.Service.Enums;

namespace Product.Service.Commands
{
    public record UpdateClienteCommand(
    Guid Id,
    string? NomeRazaoSocial = null,
    string? Email = null,
    Endereco? Endereco = null,
    TipoCliente? Tipo = null,
    DateTime? DataNascimento = null,
    string? Telefone = null,
    string? InscricaoEstadual = null,
    bool? IsentoIE = null) : IRequest;
}
