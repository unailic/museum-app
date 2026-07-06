using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Izlozbe.Commands.DodajDeloNaIzlozbu
{
    public class DodajDeloNaIzlozbuCommandHandler : IRequestHandler<DodajDeloNaIzlozbuCommand, int>
    {
        private readonly IUnitOfWork _uow;

        public DodajDeloNaIzlozbuCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<int> Handle(DodajDeloNaIzlozbuCommand request, CancellationToken cancellationToken)
        {
            var izlozba = _uow.Izlozbe.GetById(request.IzlozbaId);
            if (izlozba == null)
                throw new InvalidOperationException("Izložba ne postoji.");

            var delo = _uow.UmetnickaDela.GetById(request.UmetnickoDeloId);
            if (delo == null)
                throw new InvalidOperationException("Umetničko delo ne postoji.");

            var stavka = new StavkaIzlozbe
            {
                IzlozbaId = request.IzlozbaId,
                UmetnickoDeloId = request.UmetnickoDeloId,
                Napomena = request.Napomena
            };

            _uow.StavkeIzlozbe.Add(stavka);
            _uow.SaveChanges();

            return Task.FromResult(stavka.Id);
        }
    }
}
