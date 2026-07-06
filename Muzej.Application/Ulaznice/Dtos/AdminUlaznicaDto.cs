using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Ulaznice.Dtos
{
    public class AdminUlaznicaDto
    {
        public int Id { get; set; }
        public string NazivIzlozbe { get; set; }
        public string? PosetilacEmail { get; set; }
        public string? PosetilacImePrezime { get; set; }
        public DateTime? DatumKupovine { get; set; }
        public DateTime DatumPosete { get; set; }
        public string Status { get; set; }
        public double? CenaPlacena { get; set; }
    }
}