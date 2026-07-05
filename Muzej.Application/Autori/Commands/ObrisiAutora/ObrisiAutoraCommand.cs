using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.Autori.Commands.ObrisiAutora
{
    public class ObrisiAutoraCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
