using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Ulaznice.Queries.GetMojeUlaznice
{
    public class GetMojeUlazniceQueryHandler : IRequestHandler<GetMojeUlazniceQuery, List<Ulaznica>>
    {
        private readonly IUnitOfWork _uow;

        public GetMojeUlazniceQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<Ulaznica>> Handle(GetMojeUlazniceQuery request, CancellationToken cancellationToken)
        {
            var ulaznice = _uow.Ulaznice.Find(u => u.PosetilacId == request.PosetilacId).ToList();
            return Task.FromResult(ulaznice);
        }
    }
}
