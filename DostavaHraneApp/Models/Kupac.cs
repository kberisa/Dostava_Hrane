using DostavaHrane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DostavaHrane.Models
{
    public class Kupac : Entitet
    {
        
        public string? KorisnickoIme { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Adresa { get; set; }
        public string? Telefon { get; set; }

        public ICollection<Proizvod> KupacProizvodi { get; } = new List<Proizvod>();
        public ICollection<Dostavljac> KupacDostavljaci { get; } = new List<Dostavljac>();
        public ICollection<Kosarica> KupacKosarice { get; } = new List<Kosarica>();
    }
}
