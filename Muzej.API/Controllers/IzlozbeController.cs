using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muzej.Application.Izlozbe.Commands.DodajDeloNaIzlozbu;
using Muzej.Application.Izlozbe.Commands.IzmeniIzlozbu;
using Muzej.Application.Izlozbe.Commands.KreirajIzlozbu;
using Muzej.Application.Izlozbe.Commands.ObrisiIzlozbu;
using Muzej.Application.Izlozbe.Commands.UkloniDeloSaIzlozbe;
using Muzej.Application.Izlozbe.Queries.GetIzlozbaById;
using Muzej.Application.Izlozbe.Queries.GetIzlozbe;
using System.Runtime.Intrinsics.Arm;

namespace Muzej.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IzlozbeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IzlozbeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] KreirajIzlozbuCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id }, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetIzlozbeQuery());
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(int id, [FromBody] IzmeniIzlozbuCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id iz putanje i tela zahteva se ne poklapaju.");

            var success = await _mediator.Send(command);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new ObrisiIzlozbuCommand { Id = id });
            return success ? NoContent() : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetIzlozbaByIdQuery { Id = id });
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("{izlozbaId}/dela/{umetnickoDeloId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DodajDelo(int izlozbaId, int umetnickoDeloId, [FromBody] string? napomena = null)
        {
            var command = new DodajDeloNaIzlozbuCommand
            {
                IzlozbaId = izlozbaId,
                UmetnickoDeloId = umetnickoDeloId,
                Napomena = napomena
            };
            var id = await _mediator.Send(command);
            return Ok(new { stavkaId = id });
        }

        [HttpDelete("stavke/{stavkaId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UkloniDelo(int stavkaId)
        {
            var success = await _mediator.Send(new UkloniDeloSaIzlozbeCommand { StavkaId = stavkaId });
            return success ? NoContent() : NotFound();
        }

    }
}
