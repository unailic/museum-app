using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.Autori.Commands.KreirajAutora
{
    public class KreirajAutoraCommand : IRequest<int>
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Biografija { get; set; }
        public int GodinaRodjenja { get; set; }
    }
}