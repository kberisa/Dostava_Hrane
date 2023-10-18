using DostavaHrane1.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DostavaHrane.Models
{
    public class Kupac : Entitet
    {
        public int Sifra { get; set; }
        public string? KorisnickoIme { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Adresa { get; set; }
        public string? Telefon { get; set; }

        public ICollection<Proizvod> Proizvodi { get; } = new List<Proizvod>();
        public ICollection<Dostavljac> Dostavljaci { get; } = new List<Dostavljac>();
    }
}
