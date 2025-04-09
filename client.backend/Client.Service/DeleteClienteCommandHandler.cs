using MediatR;
using FluentValidation;
using Product.Service.Interfaces.Repositories;
using Product.Service.Interfaces;
using Product.Service.Commands;

public class DeleteClienteCommandHandler :
    IRequestHandler<DeleteClienteCommand>
{
    private readonly IClienteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteClienteCommand> _validator;

    public DeleteClienteCommandHandler(
        IClienteRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<DeleteClienteCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Handle(
        DeleteClienteCommand request,
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

        await _repository.DeleteAsync(request.Id);
        await _unitOfWork.CommitAsync();
    }
}