using Product.Service.Enums;

namespace Product.Service.Dtos
{
    public record ClienteDto(
    Guid Id,
    string NomeRazaoSocial,
    string CpfCnpj,
    string Email,
    EnderecoDto Endereco,
    TipoCliente Tipo,
    DateTime? DataNascimento,
    string? Telefone,
    string? InscricaoEstadual,
    bool IsentoIE);
}
