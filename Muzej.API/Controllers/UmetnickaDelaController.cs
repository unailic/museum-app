using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muzej.Application.UmetnickaDela.Commands.IzmeniUmetnickoDelo;
using Muzej.Application.UmetnickaDela.Commands.KreirajUmetnickoDelo;
using Muzej.Application.UmetnickaDela.Commands.ObrisiUmetnickoDelo;
using Muzej.Application.UmetnickaDela.Queries.GetUmetnickaDela;

namespace Muzej.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UmetnickaDelaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UmetnickaDelaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] KreirajUmetnickoDeloCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id }, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetUmetnickaDelaQuery());
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(int id, [FromBody] IzmeniUmetnickoDeloCommand command)
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
            var success = await _mediator.Send(new ObrisiUmetnickoDeloCommand { Id = id });
            return success ? NoContent() : NotFound();
        }
    }
}
