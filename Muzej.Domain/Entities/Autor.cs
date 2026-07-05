using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Domain.Entities
{
    public class Autor
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Biografija { get; set; }
        public int GodinaRodjenja { get; set; }

        public List<UmetnickoDelo> Dela { get; set; } = new();
    }
}
