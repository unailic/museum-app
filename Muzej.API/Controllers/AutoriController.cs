using MediatR;
using Microsoft.AspNetCore.Mvc;
using Muzej.Application.Autori.Commands.KreirajAutora;

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
        public async Task<IActionResult> Create([FromBody] KreirajAutoraCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id }, null);
        }
    }
}
