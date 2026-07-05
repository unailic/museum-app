using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Ulaznice.Commands.OtkaziUlaznicu
{
    public class OtkaziUlaznicuCommandHandler : IRequestHandler<OtkaziUlaznicuCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public OtkaziUlaznicuCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(OtkaziUlaznicuCommand request, CancellationToken cancellationToken)
        {
            var ulaznica = _uow.Ulaznice.GetById(request.Id);
            if (ulaznica == null)
                return Task.FromResult(false);

            if (ulaznica.PosetilacId != request.PosetilacId)
                throw new InvalidOperationException("Ne možete otkazati tuđu ulaznicu.");

            if (ulaznica.Status != StatusUlaznice.Kupljena)
                throw new InvalidOperationException("Samo kupljena ulaznica može biti otkazana.");

            if (ulaznica.DatumPosete < DateTime.Now)
                throw new InvalidOperationException("Ne možete otkazati ulaznicu za izložbu koja je već prošla.");

            ulaznica.Status = StatusUlaznice.Slobodna;
            ulaznica.PosetilacId = null;
            ulaznica.DatumKupovine = null;
            ulaznica.CenaPlacena = null;

            _uow.Ulaznice.Update(ulaznica);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}