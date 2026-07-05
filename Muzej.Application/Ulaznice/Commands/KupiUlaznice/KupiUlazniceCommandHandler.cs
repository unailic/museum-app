using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using Muzej.Domain.Services;

namespace Muzej.Application.Ulaznice.Commands.KupiUlaznice
{
    public class KupiUlazniceCommandHandler : IRequestHandler<KupiUlazniceCommand, List<int>>
    {
        private readonly IUnitOfWork _uow;

        public KupiUlazniceCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<int>> Handle(KupiUlazniceCommand request, CancellationToken cancellationToken)
        {
            var izlozba = _uow.Izlozbe.GetById(request.IzlozbaId);
            if (izlozba == null)
                throw new InvalidOperationException("Izložba ne postoji.");

            if (izlozba.DatumZavrsetka < DateTime.Now)
                throw new InvalidOperationException("Ne možete kupiti kartu za izložbu koja je već završena.");

            var slobodne = _uow.Ulaznice.GetSlobodneZaIzlozbu(request.IzlozbaId, request.BrojKarata).ToList();

            if (slobodne.Count < request.BrojKarata)
                throw new InvalidOperationException("Nema dovoljno slobodnih karata za ovu izložbu.");

            var popust = PopustPravila.IzracunajPopust(request.TipPosetioca);
            var cenaPoKarti = izlozba.Cena * (1 - popust);

            foreach (var ulaznica in slobodne)
            {
                ulaznica.PosetilacId = request.PosetilacId;
                ulaznica.DatumKupovine = DateTime.Now;
                ulaznica.Status = StatusUlaznice.Kupljena;
                ulaznica.CenaPlacena = cenaPoKarti;

                _uow.Ulaznice.Update(ulaznica);
            }

            _uow.SaveChanges();

            return Task.FromResult(slobodne.Select(u => u.Id).ToList());
        }
    }
}
