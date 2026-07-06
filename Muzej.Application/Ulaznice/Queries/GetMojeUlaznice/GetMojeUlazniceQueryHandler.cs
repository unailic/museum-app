using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Application.Ulaznice.Dtos;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Ulaznice.Queries.GetMojeUlaznice
{
    public class GetMojeUlazniceQueryHandler : IRequestHandler<GetMojeUlazniceQuery, List<UlaznicaDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetMojeUlazniceQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<UlaznicaDto>> Handle(GetMojeUlazniceQuery request, CancellationToken cancellationToken)
        {
            var ulaznice = _uow.Ulaznice.GetMojeUlazniceWithIzlozba(request.PosetilacId);

            var dtos = ulaznice.Select(u => new UlaznicaDto
            {
                Id = u.Id,
                IzlozbaId = u.IzlozbaId,
                NazivIzlozbe = u.Izlozba.Naziv,
                DatumKupovine = u.DatumKupovine,
                DatumPosete = u.DatumPosete,
                Status = u.Status.ToString(),
                CenaPlacena = u.CenaPlacena
            }).ToList();

            return Task.FromResult(dtos);
        }
    }
}
