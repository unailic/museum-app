using MediatR;
using Muzej.Application.Izlozbe.Dtos;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;


namespace Muzej.Application.Izlozbe.Queries.GetIzlozbaById
{
    public class GetIzlozbaByIdQueryHandler : IRequestHandler<GetIzlozbaByIdQuery, IzlozbaDetaljiDto?>
    {
        private readonly IUnitOfWork _uow;

        public GetIzlozbaByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<IzlozbaDetaljiDto?> Handle(GetIzlozbaByIdQuery request, CancellationToken cancellationToken)
        {
            var izlozba = _uow.Izlozbe.GetByIdWithDetalji(request.Id);
            if (izlozba == null)
                return Task.FromResult<IzlozbaDetaljiDto?>(null);

            var dto = new IzlozbaDetaljiDto
            {
                Id = izlozba.Id,
                Naziv = izlozba.Naziv,
                Opis = izlozba.Opis,
                DatumPocetka = izlozba.DatumPocetka,
                DatumZavrsetka = izlozba.DatumZavrsetka,
                Status = izlozba.Status.ToString(),
                Cena = izlozba.Cena,
                Kapacitet = izlozba.Kapacitet,
                BrojSlobodnihKarata = izlozba.Ulaznice.Count(u => u.Status == StatusUlaznice.Slobodna),
                NaziviDela = izlozba.StavkeIzlozbe.Select(si => si.UmetnickoDelo.Naziv).ToList()
            };

            return Task.FromResult<IzlozbaDetaljiDto?>(dto);
        }
    }
}
