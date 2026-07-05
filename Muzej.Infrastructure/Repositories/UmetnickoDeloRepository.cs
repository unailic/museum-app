using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Infrastructure.Repositories
{
    public class UmetnickoDeloRepository : Repository<UmetnickoDelo>, IUmetnickoDeloRepository
    {
        public UmetnickoDeloRepository(MuzejContext context) : base(context) { }

        public UmetnickoDelo? GetByIdWithStavke(int id) =>
            DbSet.Include(d => d.StavkeIzlozbe).FirstOrDefault(d => d.Id == id);
    }
}