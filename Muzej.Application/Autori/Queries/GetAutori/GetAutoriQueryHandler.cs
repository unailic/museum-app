using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Autori.Queries.GetAutori
{
    public class GetAutoriQueryHandler : IRequestHandler<GetAutoriQuery, List<Autor>>
    {
        private readonly IUnitOfWork _uow;

        public GetAutoriQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<Autor>> Handle(GetAutoriQuery request, CancellationToken cancellationToken)
        {
            var autori = _uow.Autori.GetAll().ToList();
            return Task.FromResult(autori);
        }
    }
}
