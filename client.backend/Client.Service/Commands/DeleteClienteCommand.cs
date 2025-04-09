using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Product.Service.Commands
{
    public record DeleteClienteCommand(Guid Id) : IRequest;

    public class DeleteClienteCommandValidator : AbstractValidator<DeleteClienteCommand>
    {
        public DeleteClienteCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do cliente é obrigatório")
                .Must(id => id != Guid.Empty).WithMessage("ID inválido");
        }
    }
}
