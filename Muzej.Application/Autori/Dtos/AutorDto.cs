using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Autori.Dtos
{
    public class AutorDto
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Biografija { get; set; }
        public int GodinaRodjenja { get; set; }
        public int BrojDela { get; set; }
    }
}