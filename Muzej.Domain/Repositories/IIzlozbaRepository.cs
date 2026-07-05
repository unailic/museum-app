using Muzej.Domain.Entities;

namespace Muzej.Domain.Repositories
{
    public interface IIzlozbaRepository : IRepository<Izlozba>
    {
        Izlozba? GetByIdWithDetalji(int id);
    }
}
