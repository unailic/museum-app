using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Izlozbe.Commands.UkloniDeloSaIzlozbe
{
    public class UkloniDeloSaIzlozbeCommandHandler : IRequestHandler<UkloniDeloSaIzlozbeCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public UkloniDeloSaIzlozbeCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(UkloniDeloSaIzlozbeCommand request, CancellationToken cancellationToken)
        {
            var stavka = _uow.StavkeIzlozbe.GetById(request.StavkaId);
            if (stavka == null)
                return Task.FromResult(false);

            _uow.StavkeIzlozbe.Remove(stavka);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}