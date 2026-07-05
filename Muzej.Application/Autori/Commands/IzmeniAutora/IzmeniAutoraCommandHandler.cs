using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Autori.Commands.IzmeniAutora
{
    public class IzmeniAutoraCommandHandler : IRequestHandler<IzmeniAutoraCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public IzmeniAutoraCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(IzmeniAutoraCommand request, CancellationToken cancellationToken)
        {
            var autor = _uow.Autori.GetById(request.Id);
            if (autor == null)
                return Task.FromResult(false);

            autor.Ime = request.Ime;
            autor.Prezime = request.Prezime;
            autor.Biografija = request.Biografija;
            autor.GodinaRodjenja = request.GodinaRodjenja;

            _uow.Autori.Update(autor);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
