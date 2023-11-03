using DostavaHrane.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DostavaHrane.Models
{
    public class Proizvod : Entitet
    {
        
        public string? Naziv { get; set; }
        public string? Opis { get; set; }
        public decimal? Cijena { get; set; }
        public bool? Dostupnost { get; set; }

        public ICollection<Kupac> ProizvodKupci { get; } = new List<Kupac>();
        public ICollection<Dostavljac> ProizvodDostavljaci { get; } = new List<Dostavljac>();
        public ICollection<Kosarica> ProizvodKosarice { get; } = new List<Kosarica>();
    }
}
