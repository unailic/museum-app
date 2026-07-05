using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Muzej.Infrastructure.Repositories
{
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(MuzejContext context) : base(context) { }

        public Autor? GetByIdWithDela(int id) =>
            DbSet.Include(a => a.Dela)
                 .FirstOrDefault(a => a.Id == id);
    }
}