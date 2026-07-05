using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.Autori.Commands.IzmeniAutora
{
    public class IzmeniAutoraCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Biografija { get; set; }
        public int GodinaRodjenja { get; set; }
    }
}
