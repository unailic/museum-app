using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.Izlozbe.Commands.DodajDeloNaIzlozbu
{
    public class DodajDeloNaIzlozbuCommand : IRequest<int>
    {
        public int IzlozbaId { get; set; }
        public int UmetnickoDeloId { get; set; }
        public string? Napomena { get; set; }
    }
}