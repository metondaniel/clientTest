using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Service.Interfaces
{
    public interface IEmailService
    {
        bool Validar(string email);
        Task EnviarNotificacaoCadastro(string email);
    }
}
