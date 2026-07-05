using System;
using System.Collections.Generic;
using System.Text;

using MediatR;
using Muzej.Domain.Entities;

namespace Muzej.Application.Izlozbe.Commands.KreirajIzlozbu
{
    public class KreirajIzlozbuCommand : IRequest<int>
    {
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public double Cena { get; set; }
        public int Kapacitet { get; set; }
    }
}
