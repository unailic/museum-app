using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.Izlozbe.Commands.IzmeniIzlozbu
{
    public class IzmeniIzlozbuCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public double Cena { get; set; }
    }
}
