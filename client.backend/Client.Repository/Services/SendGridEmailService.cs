using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Product.Service.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Product.Repository.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridClient _client;

        public SendGridEmailService(IConfiguration config)
        {
            var apiKey = config["EmailSettings:ApiKey"];
            _client = new SendGridClient(apiKey);
        }

        public bool Validar(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public async Task EnviarNotificacaoCadastro(string email)
        {
            var msg = new SendGridMessage
            {
                From = new EmailAddress("noreply@empresa.com", "Sistema de Clientes"),
                Subject = "Cadastro Realizado com Sucesso",
                PlainTextContent = "Seu cadastro foi realizado com sucesso."
            };
            msg.AddTo(new EmailAddress(email));
            await _client.SendEmailAsync(msg);
        }
    }
}
