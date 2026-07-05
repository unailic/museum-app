using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Izlozbe.Commands.IzmeniIzlozbu
{
    public class IzmeniIzlozbuCommandHandler : IRequestHandler<IzmeniIzlozbuCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public IzmeniIzlozbuCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(IzmeniIzlozbuCommand request, CancellationToken cancellationToken)
        {
            var izlozba = _uow.Izlozbe.GetById(request.Id);
            if (izlozba == null)
                return Task.FromResult(false);

            izlozba.Naziv = request.Naziv;
            izlozba.Opis = request.Opis;
            izlozba.DatumPocetka = request.DatumPocetka;
            izlozba.DatumZavrsetka = request.DatumZavrsetka;
            izlozba.Cena = request.Cena;

            _uow.Izlozbe.Update(izlozba);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
