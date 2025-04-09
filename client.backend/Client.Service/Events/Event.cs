using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Service.Events
{
    public abstract class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataOcorrencia { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; }
    }
}
