using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.UmetnickaDela.Queries.GetUmetnickaDela
{
    public class GetUmetnickaDelaQueryHandler : IRequestHandler<GetUmetnickaDelaQuery, List<UmetnickoDelo>>
    {
        private readonly IUnitOfWork _uow;

        public GetUmetnickaDelaQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<UmetnickoDelo>> Handle(GetUmetnickaDelaQuery request, CancellationToken cancellationToken)
        {
            var dela = _uow.UmetnickaDela.GetAll().ToList();
            return Task.FromResult(dela);
        }
    }
}
