using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Izlozbe.Dtos
{
    public class IzlozbaDetaljiDto
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
        public List<string> NaziviDela { get; set; } = new();
        public List<StavkaIzlozbeDto> Stavke { get; set; } = new();
    }

    public class StavkaIzlozbeDto
    {
        public int StavkaId { get; set; }
        public string NazivDela { get; set; }
    }
}