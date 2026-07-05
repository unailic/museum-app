using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Izlozbe.Queries.GetIzlozbe
{
    public class GetIzlozbeQueryHandler : IRequestHandler<GetIzlozbeQuery, List<Izlozba>>
    {
        private readonly IUnitOfWork _uow;

        public GetIzlozbeQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<Izlozba>> Handle(GetIzlozbeQuery request, CancellationToken cancellationToken)
        {
            var izlozbe = _uow.Izlozbe.GetAll().ToList();
            return Task.FromResult(izlozbe);
        }
    }
}
