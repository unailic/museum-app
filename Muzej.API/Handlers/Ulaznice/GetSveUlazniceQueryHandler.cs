using MediatR;
using Microsoft.AspNetCore.Identity;
using Muzej.Application.Ulaznice.Dtos;
using Muzej.Application.Ulaznice.Queries.GetSveUlaznice;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using Muzej.Infrastructure.Identity;

namespace Muzej.API.Handlers.Ulaznice
{
    public class GetSveUlazniceQueryHandler : IRequestHandler<GetSveUlazniceQuery, List<AdminUlaznicaDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<Korisnik> _userManager;

        public GetSveUlazniceQueryHandler(IUnitOfWork uow, UserManager<Korisnik> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        public async Task<List<AdminUlaznicaDto>> Handle(GetSveUlazniceQuery request, CancellationToken cancellationToken)
        {
            var ulaznice = _uow.Ulaznice.Find(u => u.Status != StatusUlaznice.Slobodna).ToList();
            var rezultat = new List<AdminUlaznicaDto>();

            foreach (var u in ulaznice)
            {
                var izlozba = _uow.Izlozbe.GetById(u.IzlozbaId);
                Korisnik? posetilac = u.PosetilacId != null
                    ? await _userManager.FindByIdAsync(u.PosetilacId)
                    : null;

                rezultat.Add(new AdminUlaznicaDto
                {
                    Id = u.Id,
                    NazivIzlozbe = izlozba?.Naziv ?? "Nepoznato",
                    PosetilacEmail = posetilac?.Email,
                    PosetilacImePrezime = posetilac != null ? $"{posetilac.Ime} {posetilac.Prezime}" : null,
                    DatumKupovine = u.DatumKupovine,
                    DatumPosete = u.DatumPosete,
                    Status = u.Status.ToString(),
                    CenaPlacena = u.CenaPlacena
                });
            }

            return rezultat;
        }
    }
}
