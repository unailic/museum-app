using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;

namespace Muzej.Application.UmetnickaDela.Commands.KreirajUmetnickoDelo
{
    public class KreirajUmetnickoDeloCommand : IRequest<int>
    {
        public string Naziv { get; set; }
        public int GodinaNastanka { get; set; }
        public string Opis { get; set; }
        public string ImgUrl { get; set; }
        public int AutorId { get; set; }

        public TipUmetnickogDela TipDela { get; set; }

        public string? Tehnika { get; set; }
        public string? Dimenzije { get; set; }

        public string? Materijal { get; set; }
        public double? Visina { get; set; }
    }
}
