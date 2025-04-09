using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Service.Events;

namespace Product.Service.Interfaces.EventSourcing
{
    public interface IEventStore
    {
        Task SaveEventAsync<TEvent>(TEvent @event) where TEvent : Event;
    }
}
