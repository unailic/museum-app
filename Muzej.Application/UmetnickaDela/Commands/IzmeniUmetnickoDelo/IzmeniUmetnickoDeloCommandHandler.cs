using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;

namespace Muzej.Application.UmetnickaDela.Commands.IzmeniUmetnickoDelo
{
    public class IzmeniUmetnickoDeloCommandHandler : IRequestHandler<IzmeniUmetnickoDeloCommand, bool>
    {
        private readonly IUnitOfWork _uow;

        public IzmeniUmetnickoDeloCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public Task<bool> Handle(IzmeniUmetnickoDeloCommand request, CancellationToken cancellationToken)
        {
            var delo = _uow.UmetnickaDela.GetById(request.Id);
            if (delo == null)
                return Task.FromResult(false);

            delo.Naziv = request.Naziv;
            delo.GodinaNastanka = request.GodinaNastanka;
            delo.Opis = request.Opis;
            delo.ImgUrl = request.ImgUrl;
            delo.AutorId = request.AutorId;

            if (delo is Slika slika)
            {
                slika.Tehnika = request.Tehnika;
                slika.Dimenzije = request.Dimenzije;
            }
            else if (delo is Skulptura skulptura)
            {
                skulptura.Materijal = request.Materijal;
                skulptura.Visina = request.Visina ?? skulptura.Visina;
            }

            _uow.UmetnickaDela.Update(delo);
            _uow.SaveChanges();

            return Task.FromResult(true);
        }
    }
}