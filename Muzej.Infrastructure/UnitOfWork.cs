using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using Muzej.Infrastructure.Repositories;

namespace Muzej.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MuzejContext _context;

        private IAutorRepository? _autori;
        private IUmetnickoDeloRepository? _umetnickaDela;
        private IIzlozbaRepository? _izlozbe;
        private IRepository<StavkaIzlozbe>? _stavkeIzlozbe;
        private IUlaznicaRepository? _ulaznice;

        public UnitOfWork(MuzejContext context)
        {
            _context = context;
        }

        public IAutorRepository Autori =>
            _autori ??= new AutorRepository(_context);

        public IUmetnickoDeloRepository UmetnickaDela =>
            _umetnickaDela ??= new UmetnickoDeloRepository(_context);

        public IIzlozbaRepository Izlozbe =>
            _izlozbe ??= new IzlozbaRepository(_context);

        public IRepository<StavkaIzlozbe> StavkeIzlozbe =>
            _stavkeIzlozbe ??= new Repository<StavkaIzlozbe>(_context);

        public IUlaznicaRepository Ulaznice =>
            _ulaznice ??= new UlaznicaRepository(_context);

        public int SaveChanges() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}