using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Product.Service.Entities
{
    public record Endereco(
    string Cep,
    string Logradouro,
    string Numero,
    string Bairro,
    string Cidade,
    string Estado);
}
