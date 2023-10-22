using DostavaHrane.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DostavaHrane.Models
{
    public class Kosarica : Entitet
    {
        public int Sifra { get; set; }
        [ForeignKey("Proizvod")]
        public string? Proizvod { get; set; }
        public int Kolicina { get; set; }
        [ForeignKey("Kupac")]
        public string? Kupac { get; set; }
        public string? AdresaDostave { get; set; }
        public string? StatusDostave { get; set; }
        [ForeignKey("Dostavljac")]
        public string? Dostavljac { get; set; }

        public ICollection<Kupac> Kupci { get; } = new List<Kupac>();
        public ICollection<Proizvod> Proizvodi { get; } = new List<Proizvod>();
        public ICollection<Dostavljac> Dostavljaci { get; } = new List<Dostavljac>();

    }
}
