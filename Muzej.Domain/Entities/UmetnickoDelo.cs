using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Domain.Entities
{
    public abstract class UmetnickoDelo
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int GodinaNastanka { get; set; }
        public string Opis { get; set; }
        public string ImgUrl { get; set; }

        public int AutorId { get; set; }
        public Autor Autor { get; set; }

        public List<StavkaIzlozbe> StavkeIzlozbe { get; set; } = new();
    }
}
