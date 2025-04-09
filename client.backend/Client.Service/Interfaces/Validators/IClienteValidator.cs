using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Product.Service.Interfaces.Validators
{
    public interface IClienteValidator<T>
    {
        Task<ValidationResult> ValidateCommandAsync(T command);
    }
}
