using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Domain.Entities
{
    public class StavkaIzlozbe
    {
        public int Id { get; set; }

        public int UmetnickoDeloId { get; set; }
        public UmetnickoDelo UmetnickoDelo { get; set; }

        public int IzlozbaId { get; set; }
        public Izlozba Izlozba { get; set; }

        public string? Napomena { get; set; }
    }
}
