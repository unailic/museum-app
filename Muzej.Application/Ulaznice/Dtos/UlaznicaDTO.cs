using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.Ulaznice.Dtos
{
    public class UlaznicaDto
    {
        public int Id { get; set; }
        public int IzlozbaId { get; set; }
        public string NazivIzlozbe { get; set; }
        public DateTime? DatumKupovine { get; set; }
        public DateTime DatumPosete { get; set; }
        public string Status { get; set; }
        public double? CenaPlacena { get; set; }
    }
}