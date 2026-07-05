using MediatR;
using Microsoft.AspNetCore.Mvc;
using Muzej.Application.Izlozbe.Commands.KreirajIzlozbu;
using Muzej.Application.Izlozbe.Queries.GetIzlozbe;

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
    }
}
