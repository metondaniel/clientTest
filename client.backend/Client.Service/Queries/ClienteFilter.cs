using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Service.Enums;

namespace Product.Service.Queries
{
    public class ClienteFilter
    {
        public string? NomeRazaoSocial { get; set; }
        public string? CpfCnpj { get; set; }
        public string? Email { get; set; }
        public TipoCliente? Tipo { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public DateTime? DataCadastroInicio { get; set; }
        public DateTime? DataCadastroFim { get; set; }
    }
}
