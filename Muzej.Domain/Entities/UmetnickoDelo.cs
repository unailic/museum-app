using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Muzej.Domain.Entities
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "tip")]
    [JsonDerivedType(typeof(Slika), "Slika")]
    [JsonDerivedType(typeof(Skulptura), "Skulptura")]
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
