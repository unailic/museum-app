namespace Muzej.Domain.Entities
{
    public class Ulaznica
    {
        public int Id { get; set; }

        public int IzlozbaId { get; set; }
        public Izlozba Izlozba { get; set; }

        public string? PosetilacId { get; set; }

        public DateTime? DatumKupovine { get; set; }
        public DateTime DatumPosete { get; set; }
        public StatusUlaznice Status { get; set; }
        public double? CenaPlacena { get; set; }
    }
}