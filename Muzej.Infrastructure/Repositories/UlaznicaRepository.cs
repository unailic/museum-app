using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Muzej.Infrastructure.Repositories
{
    public class UlaznicaRepository : Repository<Ulaznica>, IUlaznicaRepository
    {
        public UlaznicaRepository(MuzejContext context) : base(context) { }

        public IEnumerable<Ulaznica> GetSlobodneZaIzlozbu(int izlozbaId, int brojKarata) =>
            DbSet.Where(u => u.IzlozbaId == izlozbaId && u.Status == StatusUlaznice.Slobodna)
                 .Take(brojKarata)
                 .ToList();
    }
}
