using Microsoft.AspNetCore.Identity;
using Muzej.Domain.Entities;

namespace Muzej.Infrastructure.Identity
{
    public class Korisnik : IdentityUser
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public TipPosetioca? TipPosetioca { get; set; }
        public string? Zvanje { get; set; }

        public List<Ulaznica> Ulaznice { get; set; } = new();
    }
}