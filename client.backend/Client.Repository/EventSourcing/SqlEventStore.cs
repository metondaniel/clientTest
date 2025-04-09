using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Service.Events;
using Product.Service.Interfaces.EventSourcing;

namespace Product.Repository.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly AppDbContext _context;

        public SqlEventStore(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveEventAsync<TEvent>(TEvent @event) where TEvent : Event
        {
            await _context.EventStore.AddAsync(@event);
            await _context.SaveChangesAsync();
        }

    }
}
