using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Domain.Entities
{
    public class Skulptura : UmetnickoDelo
    {
        public string Materijal { get; set; }
        public double Visina { get; set; }
    }
}
