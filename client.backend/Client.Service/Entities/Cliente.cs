using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using Product.Service.Enums;
using Product.Service.Events;

namespace Product.Service.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string NomeRazaoSocial { get; set; }
        public string CpfCnpj { get; private set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public Endereco Endereco { get; set; } 
        public string? InscricaoEstadual { get; set; }
        public bool IsentoIE { get; set; }
        public TipoCliente Tipo { get; set; }

        public Cliente(
            string nome,
            string cpfCnpj,
            string email,
            Endereco endereco,
            string telefone,
            TipoCliente tipo,
            DateTime? dataNascimento = null,
            string? ie = null,
            bool isentoIE = false)
        {
            NomeRazaoSocial = nome;
            CpfCnpj = cpfCnpj;
            Email = email;
            Endereco = endereco;
            Tipo = tipo;
            DataNascimento = dataNascimento;
            Telefone = telefone;
            InscricaoEstadual = ie;
            IsentoIE = isentoIE;
        }

        private readonly List<ClienteEvent> _events = new();
        public IReadOnlyCollection<ClienteEvent> Events => _events.AsReadOnly();
    }
}
