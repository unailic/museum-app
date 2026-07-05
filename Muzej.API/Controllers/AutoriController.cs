using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muzej.Application.Autori.Commands.IzmeniAutora;
using Muzej.Application.Autori.Commands.KreirajAutora;
using Muzej.Application.Autori.Commands.ObrisiAutora;
using Muzej.Application.Autori.Queries.GetAutori;

namespace Muzej.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoriController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutoriController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] KreirajAutoraCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id }, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAutoriQuery());
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(int id, [FromBody] IzmeniAutoraCommand command)
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
            var success = await _mediator.Send(new ObrisiAutoraCommand { Id = id });
            return success ? NoContent() : NotFound();
        }
    }
}
