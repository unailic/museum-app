using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.Izlozbe.Commands.ObrisiIzlozbu
{
    public class ObrisiIzlozbuCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}