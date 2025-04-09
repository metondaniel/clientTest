using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Product.Service.Commands;

namespace Product.Service.Validators
{
    public class DeleteClienteValidator : AbstractValidator<DeleteClienteCommand>
    {
        public DeleteClienteValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID do cliente é obrigatório")
                .Must(id => id != Guid.Empty).WithMessage("ID inválido");
        }
    }
}
