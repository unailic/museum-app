using Muzej.Domain.Entities;

namespace Muzej.Domain.Repositories
{
    public interface IUlaznicaRepository : IRepository<Ulaznica>
    {
        IEnumerable<Ulaznica> GetSlobodneZaIzlozbu(int izlozbaId, int brojKarata);
    }
}
