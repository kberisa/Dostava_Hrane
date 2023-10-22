using DostavaHrane.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DostavaHrane.Models
{
    public class Proizvod : Entitet
    {
        public int Sifra { get; set; }
        public string? Naziv { get; set; }
        public string? Opis { get; set; }
        public decimal? Cijena { get; set; }
        public bool? Dostupnost { get; set; }

        public ICollection<Kupac> Kupci { get; } = new List<Kupac>();
        public ICollection<Dostavljac> Dostavljaci { get; } = new List<Dostavljac>();
    }
}