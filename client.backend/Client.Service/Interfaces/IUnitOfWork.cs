using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Service.Interfaces.EventSourcing;
using Product.Service.Interfaces.Repositories;

namespace Product.Service.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository ClienteRepository { get; }
        IEventStore EventStore { get; }
        Task<int> CommitAsync();
        Task RollbackAsync();
    }
}
