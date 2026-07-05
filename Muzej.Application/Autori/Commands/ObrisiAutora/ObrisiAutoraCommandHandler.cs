using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Autori.Commands.ObrisiAutora
{
    public class ObrisiAutoraCommandHandler : IRequestHandler<ObrisiAutoraCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public ObrisiAutoraCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(ObrisiAutoraCommand request, CancellationToken cancellationToken)
        {
            var autor = _uow.Autori.GetByIdWithDela(request.Id);
            if (autor == null)
                return Task.FromResult(false);

            if (autor.Dela.Any())
                throw new InvalidOperationException("Ne možete obrisati autora koji ima umetnička dela u sistemu.");

            _uow.Autori.Remove(autor);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
