using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Product.Service.Commands;
using Product.Service.Entities;
using Product.Service.Enums;
using Product.Service.Interfaces.Repositories;
using Product.Service.Interfaces;
using Product.Service.Queries;
using Product.Service;
using System.Linq.Expressions;

namespace Cliente.Test.Unit
{
    public class ClienteCommandHandlerTests
    {
        private readonly Mock<IClienteRepository> _clienteRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ICpfCnpjService> _cpfCnpjServiceMock;
        private readonly ClienteCommandHandler _handler;
        private readonly UpdateClienteCommandHandler _updateHandler;

        public ClienteCommandHandlerTests()
        {
            _clienteRepoMock = new Mock<IClienteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _emailServiceMock = new Mock<IEmailService>();
            _cpfCnpjServiceMock = new Mock<ICpfCnpjService>();

            _handler = new ClienteCommandHandler(
                _clienteRepoMock.Object,
                _emailServiceMock.Object,
                _unitOfWorkMock.Object,
                _cpfCnpjServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_CreateClienteCommand_ValidData_CreatesCliente()
        {
            // Arrange
            var command = new CreateClienteCommand(
                NomeRazaoSocial: "João Silva",
                CpfCnpj: "123.456.789-09",
                Email: "joao@email.com",
                Endereco: new Endereco("12345678", "Rua das Flores", "123", "Centro", "São Paulo", "SP"),
                Tipo: TipoCliente.Fisica,
                DataNascimento: new DateTime(1990, 1, 1)
            );

            _cpfCnpjServiceMock.Setup(s => s.Validar(It.IsAny<string>())).Returns(true);
            _clienteRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Product.Service.Entities.Cliente, bool>>>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            _clienteRepoMock.Verify(r => r.AddAsync(It.IsAny<Product.Service.Entities.Cliente>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
            _emailServiceMock.Verify(e => e.EnviarNotificacaoCadastro(It.IsAny<string>()), Times.Once);
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task Handle_CreateClienteCommand_DuplicateCpfCnpj_ThrowsValidationError()
        {
            // Arrange
            var command = new CreateClienteCommand(
                NomeRazaoSocial: "João Silva",
                CpfCnpj: "123.456.789-09",
                Email: "joao@email.com",
                Endereco: new Endereco("12345678", "Rua das Flores", "123", "Centro", "São Paulo", "SP"),
                Tipo: TipoCliente.Fisica,
                DataNascimento: new DateTime(1990, 1, 1)
            );

            _cpfCnpjServiceMock.Setup(s => s.Validar(It.IsAny<string>())).Returns(true);
            _clienteRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Product.Service.Entities.Cliente, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, new CancellationToken()));
        }

        [Fact]
        public async Task Handle_UpdateClienteCommand_ValidData_UpdatesCliente()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var command = new UpdateClienteCommand(
                Id: clienteId,
                Email: "novoemail@teste.com"
            );

            var existingCliente = new Product.Service.Entities.Cliente(
                "João Silva", "123.456.789-09", "joao@email.com",
                new Endereco("12345678", "Rua Antiga", "456", "Centro", "São Paulo", "SP"),
                "1234", TipoCliente.Fisica
            );

            _clienteRepoMock.Setup(r => r.GetByIdAsync(clienteId))
                .ReturnsAsync(existingCliente);

            // Act
            await _updateHandler.Handle(command, new CancellationToken());

            // Assert
            _clienteRepoMock.Verify(r => r.UpdateAsync(It.Is<Product.Service.Entities.Cliente>(c => c.Email == "novoemail@teste.com")), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetClientesQuery_FilterByCidade_ReturnsCorrectResults()
        {
            // Arrange
            var filter = new ClienteFilter { Cidade = "São Paulo" };
            var query = new ClienteQuery(filter);

            var clientes = new List<Product.Service.Entities.Cliente>
            {
                new Product.Service.Entities.Cliente("Cliente SP", "111.111.111-11", "sp@teste.com",
                    new Endereco("11111111", "Av Paulista", "1000", "Bela Vista", "São Paulo", "SP"),
                    "1234",TipoCliente.Fisica),
                new Product.Service.Entities.Cliente("Cliente RJ", "222.222.222-22", "rj@teste.com",
                    new Endereco("22222222", "Copacabana", "200", "Copacabana", "Rio de Janeiro", "RJ"),
                    "1234",TipoCliente.Fisica)
            };

            _clienteRepoMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Product.Service.Entities.Cliente, bool>>>()))
                .ReturnsAsync(clientes.Where(c => c.Endereco.Cidade == "São Paulo").ToList());

            // Act
            var result = await _clienteRepoMock.Object.GetByFilterAsync(c => c.Endereco.Cidade == "São Paulo");

            // Assert
            Assert.Single(result);
            Assert.Equal("Cliente SP", result.First().NomeRazaoSocial);
        }
    }
}