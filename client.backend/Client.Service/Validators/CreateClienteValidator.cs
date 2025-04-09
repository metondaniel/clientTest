using FluentValidation;
using Product.Service.Commands;
using Product.Service.Entities;
using Product.Service.Enums;
using Product.Service.Interfaces.Repositories;
using Product.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Service.Validators
{
    public class CreateClienteValidator : AbstractValidator<CreateClienteCommand>
    {
        public CreateClienteValidator(
            IClienteRepository clienteRepository,
            ICpfCnpjService cpfCnpjService)
        {
            RuleFor(x => x.NomeRazaoSocial)
                .NotEmpty().WithMessage("Nome/Razão Social é obrigatório")
                .MaximumLength(150).WithMessage("Máximo de 150 caracteres");

            RuleFor(x => x.CpfCnpj)
                .NotEmpty().WithMessage("CPF/CNPJ é obrigatório")
                .Must(cpfCnpjService.Validar).WithMessage("CPF/CNPJ inválido")
                .MustAsync(async (cpfCnpj, _) =>
                    !await clienteRepository.ExistsAsync(c => c.CpfCnpj == cpfCnpj))
                    .WithMessage("CPF/CNPJ já cadastrado");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório")
                .EmailAddress().WithMessage("E-mail inválido")
                .MustAsync(async (email, _) =>
                    !await clienteRepository.ExistsAsync(c => c.Email == email))
                    .WithMessage("E-mail já cadastrado");

            When(x => x.Tipo == TipoCliente.Fisica, () =>
            {
                RuleFor(x => x.DataNascimento)
                    .NotNull().WithMessage("Data de nascimento obrigatória")
                    .Must(BeAtLeast18YearsOld).WithMessage("Idade mínima 18 anos");
            });

            When(x => x.Tipo == TipoCliente.Juridica, () =>
            {
                RuleFor(x => x)
                    .Must(c => c.IsentoIE || !string.IsNullOrEmpty(c.InscricaoEstadual))
                    .WithMessage("IE obrigatório ou marcar como isento");
            });
        }

        private bool BeAtLeast18YearsOld(DateTime? dataNascimento)
        {
            if (!dataNascimento.HasValue) return false;
            var idade = DateTime.Today.Year - dataNascimento.Value.Year;
            if (dataNascimento.Value.Date > DateTime.Today.AddYears(-idade)) idade--;
            return idade >= 18;
        }
    }
}
