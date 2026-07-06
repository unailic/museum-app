using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Application.Autori.Dtos;
using Muzej.Domain.Repositories;

namespace Muzej.Application.Autori.Queries.GetAutori
{
    public class GetAutoriQueryHandler : IRequestHandler<GetAutoriQuery, List<AutorDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetAutoriQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<AutorDto>> Handle(GetAutoriQuery request, CancellationToken cancellationToken)
        {
            var autori = _uow.Autori.GetAllWithDela();

            var dtos = autori.Select(a => new AutorDto
            {
                Id = a.Id,
                Ime = a.Ime,
                Prezime = a.Prezime,
                Biografija = a.Biografija,
                GodinaRodjenja = a.GodinaRodjenja,
                BrojDela = a.Dela.Count
            }).ToList();

            return Task.FromResult(dtos);
        }
    }
}
