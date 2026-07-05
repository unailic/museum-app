using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.UmetnickaDela.Commands.ObrisiUmetnickoDelo
{
    public class ObrisiUmetnickoDeloCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
