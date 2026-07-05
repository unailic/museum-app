using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Izlozbe.Commands.KreirajIzlozbu
{
    public class KreirajIzlozbuCommandHandler : IRequestHandler<KreirajIzlozbuCommand, int>
    {
        private readonly IUnitOfWork _uow;

        public KreirajIzlozbuCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<int> Handle(KreirajIzlozbuCommand request, CancellationToken cancellationToken)
        {
            var izlozba = new Izlozba
            {
                Naziv = request.Naziv,
                Opis = request.Opis,
                DatumPocetka = request.DatumPocetka,
                DatumZavrsetka = request.DatumZavrsetka,
                Cena = request.Cena,
                Kapacitet = request.Kapacitet,
                Status = StatusIzlozbe.Najavljena
            };

            _uow.Izlozbe.Add(izlozba);
            _uow.SaveChanges();

            for (int i = 0; i < request.Kapacitet; i++)
            {
                var ulaznica = new Ulaznica
                {
                    IzlozbaId = izlozba.Id,
                    DatumPosete = request.DatumPocetka,
                    Status = StatusUlaznice.Slobodna
                };
                _uow.Ulaznice.Add(ulaznica);
            }

            _uow.SaveChanges();

            return Task.FromResult(izlozba.Id);
        }
    }
}
