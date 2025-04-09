using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Product.Service.Commands;

namespace Product.Service.Validators
{
    public class UpdateClienteValidator : AbstractValidator<UpdateClienteCommand>
    {
        public UpdateClienteValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID do cliente é obrigatório");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("E-mail inválido");
        }
    }
}
