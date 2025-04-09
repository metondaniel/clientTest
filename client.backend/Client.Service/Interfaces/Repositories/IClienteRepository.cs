using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Service.Interfaces.Repositories;
using Product.Service.Entities;
using System.Linq.Expressions;

namespace Product.Service.Interfaces.Repositories
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<Cliente> GetByCpfCnpjAsync(string cpfCnpj);
        Task<Cliente> GetByEmailAsync(string email);
        Task<IEnumerable<Cliente>> GetByFilterAsync(Expression<Func<Cliente, bool>> filter);
    }
}
