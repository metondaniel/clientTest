using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Repository.EventSourcing;
using Product.Service.Interfaces.EventSourcing;
using Product.Service.Interfaces.Repositories;
using Product.Service.Interfaces;

namespace Product.Repository.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ClienteRepository = new ClienteRepository(_context);
            EventStore = new SqlEventStore(_context);
        }

        public IClienteRepository ClienteRepository { get; }
        public IEventStore EventStore { get; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
