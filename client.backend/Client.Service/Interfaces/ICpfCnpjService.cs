using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Service.Interfaces
{
    public interface ICpfCnpjService
    {
        bool Validar(string cpfCnpj);
        string Formatar(string cpfCnpj);
    }
}
