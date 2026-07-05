using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Domain.Entities
{
    public class Izlozba
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public StatusIzlozbe Status { get; set; }
        public double Cena { get; set; }
        public int Kapacitet { get; set; }

        public List<StavkaIzlozbe> StavkeIzlozbe { get; set; } = new();
        public List<Ulaznica> Ulaznice { get; set; } = new();
    }
}
