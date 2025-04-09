using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;
using Product.Service.Entities;
using Product.Service.Enums;

namespace Product.Service.Commands
{
    public record CreateClienteCommand(
    string NomeRazaoSocial,
    string CpfCnpj,
    string Email,
    Endereco Endereco,
    TipoCliente Tipo,
    DateTime? DataNascimento = null,
    string Telefone = null,
    string InscricaoEstadual = null,
    bool IsentoIE = false) : IRequest<Guid>;
}
