using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.UmetnickaDela.Commands.KreirajUmetnickoDelo
{
    public class KreirajUmetnickoDeloCommandHandler : IRequestHandler<KreirajUmetnickoDeloCommand, int>
    {
        private readonly IUnitOfWork _uow;

        public KreirajUmetnickoDeloCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<int> Handle(KreirajUmetnickoDeloCommand request, CancellationToken cancellationToken)
        {
            UmetnickoDelo delo = request.TipDela switch
            {
                TipUmetnickogDela.Slika => new Slika
                {
                    Tehnika = request.Tehnika,
                    Dimenzije = request.Dimenzije
                },
                TipUmetnickogDela.Skulptura => new Skulptura
                {
                    Materijal = request.Materijal,
                    Visina = request.Visina ?? 0
                },
                _ => throw new ArgumentException("Nepoznat tip umetničkog dela")
            };

            delo.Naziv = request.Naziv;
            delo.GodinaNastanka = request.GodinaNastanka;
            delo.Opis = request.Opis;
            delo.ImgUrl = request.ImgUrl;
            delo.AutorId = request.AutorId;

            _uow.UmetnickaDela.Add(delo);
            _uow.SaveChanges();

            return Task.FromResult(delo.Id);
        }
    }
}
