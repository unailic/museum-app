using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Muzej.Application.Ulaznice.Commands.OtkaziUlaznicu
{
    public class OtkaziUlaznicuCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string PosetilacId { get; set; }
    }
}
