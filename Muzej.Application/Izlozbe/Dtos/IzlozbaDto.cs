using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Izlozbe.Dtos
{
    public class IzlozbaDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public string Status { get; set; }
        public double Cena { get; set; }
        public int Kapacitet { get; set; }
        public int BrojSlobodnihKarata { get; set; }
    }
}
