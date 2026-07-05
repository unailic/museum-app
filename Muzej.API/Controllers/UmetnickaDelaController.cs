using MediatR;
using Microsoft.AspNetCore.Mvc;
using Muzej.Application.UmetnickaDela.Commands.KreirajUmetnickoDelo;
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
    }
}
