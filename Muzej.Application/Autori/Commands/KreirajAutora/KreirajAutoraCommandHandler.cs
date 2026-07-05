using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Autori.Commands.KreirajAutora
{
    public class KreirajAutoraCommandHandler : IRequestHandler<KreirajAutoraCommand, int>
    {
        private readonly IUnitOfWork _uow;

        public KreirajAutoraCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<int> Handle(KreirajAutoraCommand request, CancellationToken cancellationToken)
        {
            var autor = new Autor
            {
                Ime = request.Ime,
                Prezime = request.Prezime,
                Biografija = request.Biografija,
                GodinaRodjenja = request.GodinaRodjenja
            };

            _uow.Autori.Add(autor);
            _uow.SaveChanges();

            return Task.FromResult(autor.Id);
        }
    }
}
