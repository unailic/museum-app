using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.Izlozbe.Commands.UkloniDeloSaIzlozbe
{
    public class UkloniDeloSaIzlozbeCommand : IRequest<bool>
    {
        public int StavkaId { get; set; }
    }
}