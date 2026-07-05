using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Repositories;

namespace Muzej.Application.UmetnickaDela.Commands.ObrisiUmetnickoDelo
{
    public class ObrisiUmetnickoDeloCommandHandler : IRequestHandler<ObrisiUmetnickoDeloCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public ObrisiUmetnickoDeloCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(ObrisiUmetnickoDeloCommand request, CancellationToken cancellationToken)
        {
            var delo = _uow.UmetnickaDela.GetByIdWithStavke(request.Id);
            if (delo == null)
                return Task.FromResult(false);

            if (delo.StavkeIzlozbe.Any())
                throw new InvalidOperationException("Ne možete obrisati delo koje je trenutno izlagano na nekoj izložbi.");

            _uow.UmetnickaDela.Remove(delo);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
