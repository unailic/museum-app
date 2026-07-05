using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muzej.Application.Ulaznice.Commands.KupiUlaznice;
using Muzej.Application.Ulaznice.Commands.OtkaziUlaznicu;
using Muzej.Application.Ulaznice.Queries.GetMojeUlaznice;
using Muzej.Domain.Entities;
using System.Security.Claims;

namespace Muzej.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UlazniceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UlazniceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Kupi([FromBody] KupiUlazniceRequest request)
        {
            var posetilacId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var command = new KupiUlazniceCommand
            {
                PosetilacId = posetilacId,
                TipPosetioca = request.TipPosetioca,
                IzlozbaId = request.IzlozbaId,
                BrojKarata = request.BrojKarata
            };

            var ids = await _mediator.Send(command);
            return Ok(ids);
        }

        [HttpGet("moje")]
        public async Task<IActionResult> MojeUlaznice()
        {
            var posetilacId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(new GetMojeUlazniceQuery { PosetilacId = posetilacId });
            return Ok(result);
        }

        [HttpPut("{id}/otkazi")]
        public async Task<IActionResult> Otkazi(int id)
        {
            var posetilacId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _mediator.Send(new OtkaziUlaznicuCommand { Id = id, PosetilacId = posetilacId });
            return success ? NoContent() : NotFound();
        }
    }

    public record KupiUlazniceRequest(TipPosetioca TipPosetioca, int IzlozbaId, int BrojKarata);
}