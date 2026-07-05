using MediatR;
using Microsoft.AspNetCore.Mvc;
using Muzej.Application.Ulaznice.Commands.KupiUlaznice;
using Muzej.Application.Ulaznice.Queries.GetMojeUlaznice;

namespace Muzej.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UlazniceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UlazniceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Kupi([FromBody] KupiUlazniceCommand command)
        {
            try
            {
                var ids = await _mediator.Send(command);
                return Ok(ids);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { poruka = ex.Message });
            }
        }

        [HttpGet("moje/{posetilacId}")]
        public async Task<IActionResult> MojeUlaznice(string posetilacId)
        {
            var result = await _mediator.Send(new GetMojeUlazniceQuery { PosetilacId = posetilacId });
            return Ok(result);
        }
    }
}