namespace Product.Service.Dtos
{
    public record EnderecoDto(
    string Cep,
    string Logradouro,
    string Numero,
    string Bairro,
    string Cidade,
    string Estado);
}
