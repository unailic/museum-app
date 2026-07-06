using System;
using System.Collections.Generic;
using System.Text;

namespace Muzej.Application.UmetnickaDela.Dtos
{
    public class UmetnickoDeloDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int GodinaNastanka { get; set; }
        public string Opis { get; set; }
        public string ImgUrl { get; set; }
        public string Tip { get; set; }
        public string AutorImePrezime { get; set; }
        public string? Tehnika { get; set; }
        public string? Dimenzije { get; set; }
        public string? Materijal { get; set; }
        public double? Visina { get; set; }
    }
}
