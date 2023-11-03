using DostavaHrane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DostavaHrane.Models
{
    public class Kosarica : Entitet
    {

        [ForeignKey("Proizvod")]
        public int ProizvodSifra { get; set; }

        [ForeignKey("Kupac")]
        public int KupacSifra { get; set; }

        [ForeignKey("Dostavljac")]
        public int DostavljacSifra { get; set; }

        public int Kolicina { get; set; }
        public string AdresaDostave { get; set; }
        public string StatusDostave { get; set; }

        public Proizvod Proizvod { get; set; }
        public Kupac Kupac { get; set; }
        public Dostavljac Dostavljac { get; set; }

        public ICollection<Kupac> KosaricaKupci { get; } = new List<Kupac>();
        public ICollection<Proizvod> KosaricaProizvodi { get; } = new List<Proizvod>();
        public ICollection<Dostavljac> KosaricaDostavljaci { get; } = new List<Dostavljac>();

    }
}
