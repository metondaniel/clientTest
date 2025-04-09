using Amazon.S3;
using Amazon.S3.Transfer;
using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Product.Service.Commands;
using Product.Service.Entities;
using Product.Service.Events;
using Product.Service.Interfaces;
using Product.Service.Interfaces.Repositories;
using Product.Service.Interfaces.Services;
using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Product.Service
{
    public class ClienteCommandHandler :
    IRequestHandler<CreateClienteCommand, Guid>
    {
        private readonly IClienteRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICpfCnpjService _cpfCnpjService;

        public ClienteCommandHandler(
            IClienteRepository repository,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            ICpfCnpjService cpfCnpjService)
        {
            _repository = repository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _cpfCnpjService = cpfCnpjService;
        }


        public async Task<Guid> Handle(
        CreateClienteCommand request,
        CancellationToken cancellationToken)
        {
            // Formatar e validar CPF/CNPJ
            var cpfCnpjFormatado = _cpfCnpjService.Formatar(request.CpfCnpj);

            var cliente = new Cliente(
                request.NomeRazaoSocial,
                cpfCnpjFormatado,
                request.Email,
                request.Endereco,
                request.Telefone,
                request.Tipo,
                request.DataNascimento,
                request.InscricaoEstadual,
                request.IsentoIE);

            await _repository.AddAsync(cliente);
            await _unitOfWork.CommitAsync();

            _emailService.EnviarNotificacaoCadastro(cliente.Email);

            return cliente.Id;
        }
    }
}
