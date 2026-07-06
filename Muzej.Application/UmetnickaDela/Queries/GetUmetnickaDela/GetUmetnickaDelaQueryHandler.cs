using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Application.UmetnickaDela.Dtos;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.UmetnickaDela.Queries.GetUmetnickaDela
{
    public class GetUmetnickaDelaQueryHandler : IRequestHandler<GetUmetnickaDelaQuery, List<UmetnickoDeloDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetUmetnickaDelaQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<List<UmetnickoDeloDto>> Handle(GetUmetnickaDelaQuery request, CancellationToken cancellationToken)
        {
            var dela = _uow.UmetnickaDela.GetAllWithAutor();

            var dtos = dela.Select(d => new UmetnickoDeloDto
            {
                Id = d.Id,
                Naziv = d.Naziv,
                GodinaNastanka = d.GodinaNastanka,
                Opis = d.Opis,
                ImgUrl = d.ImgUrl,
                Tip = d is Slika ? "Slika" : "Skulptura",
                AutorImePrezime = $"{d.Autor.Ime} {d.Autor.Prezime}",
                Tehnika = (d as Slika)?.Tehnika,
                Dimenzije = (d as Slika)?.Dimenzije,
                Materijal = (d as Skulptura)?.Materijal,
                Visina = (d as Skulptura)?.Visina
            }).ToList();

            return Task.FromResult(dtos);
        }
    }
}
