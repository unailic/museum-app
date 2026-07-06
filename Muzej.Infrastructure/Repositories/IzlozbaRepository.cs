using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Muzej.Infrastructure.Repositories
{
    public class IzlozbaRepository : Repository<Izlozba>, IIzlozbaRepository
    {
        public IzlozbaRepository(MuzejContext context) : base(context) { }

        public Izlozba? GetByIdWithDetalji(int id) =>
            DbSet.Include(i => i.StavkeIzlozbe)
                    .ThenInclude(si => si.UmetnickoDelo)
                 .Include(i => i.Ulaznice)
                 .FirstOrDefault(i => i.Id == id);

        public IEnumerable<Izlozba> GetAllWithUlaznice() =>
                                        DbSet.Include(i => i.Ulaznice).ToList();
    }
}
