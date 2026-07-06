using Muzej.Domain.Entities;
using Muzej.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Muzej.API.BackgroundServices
{
    public class IzlozbaStatusService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public IzlozbaStatusService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MuzejContext>();
                    var sada = DateTime.Now;

                    var izlozbe = await context.Set<Izlozba>().ToListAsync(stoppingToken);

                    foreach (var izlozba in izlozbe)
                    {
                        var noviStatus = OdrediStatus(izlozba, sada);
                        if (izlozba.Status != noviStatus)
                        {
                            izlozba.Status = noviStatus;
                        }
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }

        private static StatusIzlozbe OdrediStatus(Izlozba izlozba, DateTime sada)
        {
            if (sada < izlozba.DatumPocetka)
                return StatusIzlozbe.Najavljena;
            if (sada >= izlozba.DatumPocetka && sada <= izlozba.DatumZavrsetka)
                return StatusIzlozbe.Aktivna;
            return StatusIzlozbe.Zavrsena;
        }
    }
}
