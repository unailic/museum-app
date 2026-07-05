using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Domain.Entities
{
    public class Slika : UmetnickoDelo
    {
        public string Tehnika { get; set; }
        public string Dimenzije { get; set; }
    }
}
