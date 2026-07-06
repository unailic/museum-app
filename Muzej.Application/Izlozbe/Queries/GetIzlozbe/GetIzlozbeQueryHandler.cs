using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Application.Izlozbe.Dtos;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Izlozbe.Queries.GetIzlozbe
{
    public class GetIzlozbeQueryHandler : IRequestHandler<GetIzlozbeQuery, List<IzlozbaDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetIzlozbeQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<IzlozbaDto>> Handle(GetIzlozbeQuery request, CancellationToken cancellationToken)
        {
            var izlozbe = _uow.Izlozbe.GetAllWithUlaznice();

            var dtos = izlozbe.Select(i => new IzlozbaDto
            {
                Id = i.Id,
                Naziv = i.Naziv,
                Opis = i.Opis,
                DatumPocetka = i.DatumPocetka,
                DatumZavrsetka = i.DatumZavrsetka,
                Status = i.Status.ToString(),
                Cena = i.Cena,
                Kapacitet = i.Kapacitet,
                BrojSlobodnihKarata = i.Ulaznice.Count(u => u.Status == StatusUlaznice.Slobodna)
            }).ToList();

            return Task.FromResult(dtos);
        }
    }
}
