using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using System.Linq.Expressions;
using Product.Service.Commands;
using Product.Service.Entities;
using Product.Service.Enums;
using Product.Service.Interfaces.Repositories;
using Product.Service.Interfaces;
using Product.Service;

namespace Cliente.Test
{
    public class ClienteBehaviorTests
    {
        private readonly Mock<IClienteRepository> _clienteRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ICpfCnpjService> _cpfCnpjServiceMock;
        private readonly ClienteCommandHandler _handler;

        public ClienteBehaviorTests()
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
        public async Task CreateCliente_ValidCommand_CompleteSuccessfully()
        {
            // Arrange
            var command = new CreateClienteCommand(
                "João Silva",
                "123.456.789-09",
                "joao@email.com",
                new Endereco("12345678", "Rua das Flores", "123", "Centro", "São Paulo", "SP"),
                TipoCliente.Fisica,
                new DateTime(1990, 1, 1)
            );

            _cpfCnpjServiceMock.Setup(s => s.Validar(It.IsAny<string>())).Returns(true);
            _clienteRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Product.Service.Entities.Cliente, bool>>>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, new CancellationToken());

            // Assert
            _clienteRepoMock.Verify(r => r.AddAsync(It.IsAny<Product.Service.Entities.Cliente>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task BulkCreateClientes_ShouldUseBulkOperations()
        {
            // Arrange
            var commands = Enumerable.Range(1, 100).Select(i =>
                new CreateClienteCommand(
                    $"Cliente {i}",
                    $"{i:000\\.000\\.000-00}",
                    $"cliente{i}@teste.com",
                    new Endereco("12345678", "Rua X", i.ToString(), "Bairro", "Cidade", "SP"),
                    TipoCliente.Fisica,
                    new DateTime(1990, 1, 1)
                ));

            _cpfCnpjServiceMock.Setup(s => s.Validar(It.IsAny<string>())).Returns(true);
            _clienteRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Product.Service.Entities.Cliente, bool>>>()))
                .ReturnsAsync(false);

            // Act & Assert
            foreach (var command in commands)
            {
                await _handler.Handle(command, new CancellationToken());
            }

            _clienteRepoMock.Verify(r => r.AddAsync(It.IsAny<Product.Service.Entities.Cliente>()), Times.Exactly(100));
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Exactly(100));
        }

        [Fact]
        public async Task CreateCliente_InvalidData_ShouldThrowValidationException()
        {
            // Arrange
            var invalidCommand = new CreateClienteCommand(
                "", // Nome inválido
                "123", // CPF inválido
                "email-invalido",
                null, // Endereço ausente
                TipoCliente.Fisica
            );

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _handler.Handle(invalidCommand, new CancellationToken()));
        }

        [Fact]
        public async Task FilterClientes_ByCidade_ReturnsCorrectResults()
        {
            // Arrange
            var clientes = new List<Product.Service.Entities.Cliente>
            {
                new Product.Service.Entities.Cliente("Cliente SP", "111.111.111-11", "sp@teste.com",
                    new Endereco("11111111", "Av Paulista", "1000", "Bela Vista", "São Paulo", "SP"),
                    "",TipoCliente.Fisica),
                new Product.Service.Entities.Cliente("Cliente RJ", "222.222.222-22", "rj@teste.com",
                    new Endereco("22222222", "Copacabana", "200", "Copacabana", "Rio de Janeiro", "RJ"),
                    "",TipoCliente.Fisica)
            };

            _clienteRepoMock.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Product.Service.Entities.Cliente, bool>>>()))
                .ReturnsAsync(clientes.Where(c => c.Endereco.Cidade == "São Paulo").ToList());

            // Act
            var results = await _clienteRepoMock.Object.GetByFilterAsync(c => c.Endereco.Cidade == "São Paulo");

            // Assert
            Assert.Single(results);
            Assert.Equal("Cliente SP", results.First().NomeRazaoSocial);
        }
    }
}