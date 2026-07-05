using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Izlozbe.Commands.ObrisiIzlozbu
{
    public class ObrisiIzlozbuCommandHandler : IRequestHandler<ObrisiIzlozbuCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public ObrisiIzlozbuCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(ObrisiIzlozbuCommand request, CancellationToken cancellationToken)
        {
            var izlozba = _uow.Izlozbe.GetByIdWithDetalji(request.Id);
            if (izlozba == null)
                return Task.FromResult(false);

            var imaKupljenihKarata = izlozba.Ulaznice.Any(u => u.Status == StatusUlaznice.Kupljena);
            if (imaKupljenihKarata)
                throw new InvalidOperationException("Ne možete obrisati izložbu koja ima kupljene ulaznice.");

            _uow.Izlozbe.Remove(izlozba);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
