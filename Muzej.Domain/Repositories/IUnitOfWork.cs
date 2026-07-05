using Muzej.Domain.Entities;

namespace Muzej.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAutorRepository Autori { get; }
        IUmetnickoDeloRepository UmetnickaDela { get; }
        IIzlozbaRepository Izlozbe { get; }
        IRepository<StavkaIzlozbe> StavkeIzlozbe { get; }
        IUlaznicaRepository Ulaznice { get; }

        int SaveChanges();
    }
}
