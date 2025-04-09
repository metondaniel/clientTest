using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product.Service.Entities;
using Product.Service.Enums;

namespace Product.Service.Events
{
    public class ClienteCreatedEvent : Event
    {
        public ClienteCreatedEvent()
        {
            EventType = "ClienteCreatedEvent";
        }

        public string NomeRazaoSocial { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public Endereco Endereco { get; set; }
        public string InscricaoEstadual { get; set; }
        public bool IsentoIE { get; set; }
        public TipoCliente Tipo { get; set; }
    }
}
