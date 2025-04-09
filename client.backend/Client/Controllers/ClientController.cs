using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Service;
using Product.Service.Commands;
using Product.Service.Dtos;
using Product.Service.Interfaces;
using Product.Service.Interfaces.Services;
using Product.Service.Queries;

namespace Product.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICpfCnpjService _cpfCnpjService;

        public ClienteController(IMediator mediator, ICpfCnpjService cpfCnpjService)
        {
            _mediator = mediator;
            _cpfCnpjService = cpfCnpjService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClienteCommand command)
        {
            try
            {
                var clienteId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = clienteId }, new { Id = clienteId });
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetById(Guid id)
        {
            var query = new GetClienteByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ClienteDto>>> GetAll(
            [FromQuery] ClienteFilter filter,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new ClienteQuery(filter, pageNumber);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClienteCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var command = new DeleteClienteCommand(id);
        //    await _mediator.Send(command);
        //    return NoContent();
        //}

        [HttpGet("cpfcnpj/{cpfCnpj}/exists")]
        public async Task<ActionResult<bool>> CheckCpfCnpjExists(string cpfCnpj)
        {
            var query = _cpfCnpjService.Validar(cpfCnpj);
            return Ok(await _mediator.Send(query));
        }
    }
}
