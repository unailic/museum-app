using System;
using System.Collections.Generic;
using System.Text;

using Muzej.Domain.Entities;

namespace Muzej.Domain.Repositories
{
    public interface IUmetnickoDeloRepository : IRepository<UmetnickoDelo>
    {
        UmetnickoDelo? GetByIdWithStavke(int id);
        IEnumerable<UmetnickoDelo> GetAllWithAutor();
    }
}
