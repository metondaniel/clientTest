using MediatR;
using FluentValidation;
using System.Linq.Expressions;
using Product.Service.Commands;
using Product.Service.Enums;
using Product.Service.Interfaces.Repositories;
using Product.Service.Interfaces;

public class UpdateClienteCommandHandler :
    IRequestHandler<UpdateClienteCommand>
{
    private readonly IClienteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IValidator<UpdateClienteCommand> _validator;

    public UpdateClienteCommandHandler(
        IClienteRepository repository,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IValidator<UpdateClienteCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _validator = validator;
    }

    public async Task Handle(
        UpdateClienteCommand request,
        CancellationToken cancellationToken)
    {
        // Validação do comando
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var cliente = await _repository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Cliente não encontrado");

        // Aplicar atualizações parciais
        if (!string.IsNullOrEmpty(request.NomeRazaoSocial))
            cliente.NomeRazaoSocial = request.NomeRazaoSocial;

        if (!string.IsNullOrEmpty(request.Email))
        {
            cliente.Email = request.Email;
            await _emailService.EnviarNotificacaoCadastro(cliente.Email);
        }

        if (request.Endereco != null)
            cliente.Endereco = request.Endereco;

        if (request.Tipo.HasValue)
            cliente.Tipo = request.Tipo.Value;

        if (request.DataNascimento.HasValue)
            cliente.DataNascimento = request.DataNascimento;

        if (!string.IsNullOrEmpty(request.Telefone))
            cliente.Telefone = request.Telefone;

        if (request.Tipo == TipoCliente.Juridica)
        {
            cliente.InscricaoEstadual = request.InscricaoEstadual;
            cliente.IsentoIE = request.IsentoIE ?? false;
        }

        _repository.UpdateAsync(cliente);
        await _unitOfWork.CommitAsync();
    }
}