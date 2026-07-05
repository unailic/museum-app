using Muzej.Domain.Entities;

namespace Muzej.Domain.Repositories
{
    public interface IAutorRepository : IRepository<Autor>
    {
        Autor? GetByIdWithDela(int id);
    }
}
